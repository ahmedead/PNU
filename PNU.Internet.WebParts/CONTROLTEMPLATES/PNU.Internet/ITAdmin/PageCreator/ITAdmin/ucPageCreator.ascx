<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPageCreator.ascx.cs"
    Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ITAdmin.ucPageCreator" %>

<!-- ============ Page Creator (DGA) ============ -->
<section class="container my-5" id="pnu-page-creator">

    <!-- Access denied -->
    <asp:Panel ID="pnlDenied" runat="server" Visible="false">
        <div class="alert alert-danger d-flex align-items-center gap-2" role="alert">
            <i class="hgi hgi-stroke hgi-alert-circle fs-4"></i>
            <asp:Literal ID="litDeniedMsg" runat="server" />
        </div>
    </asp:Panel>

    <!-- Creator form -->
    <asp:Panel ID="pnlForm" runat="server" Visible="false">
        <div class="card border-0 shadow-sm rounded-3">
            <div class="card-header bg-primary-25 border-0 rounded-top-3 py-3">
                <h1 class="h4 mb-0 d-flex align-items-center gap-2">
                    <i class="hgi hgi-stroke hgi-file-add fs-4"></i>
                    <asp:Literal ID="litPageHeader" runat="server" />
                </h1>
            </div>
            <div class="card-body p-4">

                <asp:Panel ID="pnlValidation" runat="server" Visible="false" CssClass="alert alert-warning" role="alert">
                    <asp:Literal ID="litValidationMsg" runat="server" />
                </asp:Panel>

                <div class="row g-4">

                    <!-- WebSite URL -->
                    <div class="col-12 col-md-6">
                        <asp:Label ID="lblWebSiteUrl" runat="server" AssociatedControlID="txtWebSiteUrl" CssClass="form-label fw-semibold" />
                        <asp:TextBox ID="txtWebSiteUrl" runat="server" CssClass="form-control" dir="ltr" placeholder="/ar/Faculties/Science" />
                        <div class="form-text"><asp:Literal ID="litWebSiteUrlHint" runat="server" /></div>
                    </div>

                    <!-- Page Template -->
                    <div class="col-12 col-md-6">
                        <asp:Label ID="lblTemplate" runat="server" AssociatedControlID="ddlTemplate" CssClass="form-label fw-semibold" />
                        <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" />
                        <div class="form-text"><asp:Literal ID="litTemplateHint" runat="server" /></div>
                    </div>

                    <!-- Page Layout -->
                    <div class="col-12 col-md-6">
                        <asp:Label ID="lblPageLayout" runat="server" AssociatedControlID="ddlPageLayout" CssClass="form-label fw-semibold" />
                        <asp:DropDownList ID="ddlPageLayout" runat="server" CssClass="form-select" />
                        <div class="form-text"><asp:Literal ID="litLayoutHint" runat="server" /></div>
                    </div>

                    <!-- Page Name -->
                    <div class="col-12 col-md-6">
                        <asp:Label ID="lblPageName" runat="server" AssociatedControlID="txtPageName" CssClass="form-label fw-semibold" />
                        <div class="input-group" dir="ltr">
                            <asp:TextBox ID="txtPageName" runat="server" CssClass="form-control" placeholder="college-programs" />
                            <span class="input-group-text">.aspx</span>
                        </div>
                    </div>

                    <!-- Titles -->
                    <div class="col-12 col-md-6">
                        <asp:Label ID="lblTitleAr" runat="server" AssociatedControlID="txtPageTitleAr" CssClass="form-label fw-semibold" />
                        <asp:TextBox ID="txtPageTitleAr" runat="server" CssClass="form-control" dir="rtl" />
                    </div>
                    <div class="col-12 col-md-6">
                        <asp:Label ID="lblTitleEn" runat="server" AssociatedControlID="txtPageTitleEn" CssClass="form-label fw-semibold" />
                        <asp:TextBox ID="txtPageTitleEn" runat="server" CssClass="form-control" dir="ltr" />
                    </div>

                    <!-- User Control Properties -->
                    <div class="col-12">
                        <asp:Label ID="lblUcProperties" runat="server" AssociatedControlID="txtUcProperties" CssClass="form-label fw-semibold" />
                        <asp:TextBox ID="txtUcProperties" runat="server" CssClass="form-control font-monospace" TextMode="MultiLine" Rows="3" dir="ltr" placeholder="RowsCount=6;ShowTitle=true" />
                        <div class="form-text"><asp:Literal ID="litUcPropertiesHint" runat="server" /></div>
                    </div>

                    <!-- Options -->
                    <div class="col-12 d-flex flex-wrap gap-4">
                        <div class="form-check form-switch">
                            <asp:CheckBox ID="chkBothSites" runat="server" CssClass="form-check-input" />
                            <asp:Label ID="lblBothSites" runat="server" AssociatedControlID="chkBothSites" CssClass="form-check-label" />
                        </div>
                    </div>

                    <!-- Submit -->
                    <div class="col-12">
                        <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-primary px-5"
                            OnClick="btnCreate_Click" />
                    </div>
                </div>

                <asp:HiddenField ID="hfUcPath" runat="server" />

                <!-- Results -->
                <asp:Panel ID="pnlResults" runat="server" Visible="false" CssClass="mt-4">
                    <h2 class="h6 fw-bold"><asp:Literal ID="litResultsHeader" runat="server" /></h2>
                    <asp:Repeater ID="rptResults" runat="server">
                        <ItemTemplate>
                            <div class='alert <%# Eval("CssClass") %> py-2 px-3 mb-2' role="alert" dir="ltr">
                                <%# Eval("Message") %>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </asp:Panel>

            </div>
        </div>
    </asp:Panel>

</section>
