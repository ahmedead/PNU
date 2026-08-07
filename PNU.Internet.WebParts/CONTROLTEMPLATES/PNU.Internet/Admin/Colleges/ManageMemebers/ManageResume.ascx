<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageResume.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers.ManageResume" %>



<div class="Resumemessagealert" id="Resume_alert_container" runat="server"></div>

<div id="dvMain" runat="server">

    <article class="card mb-4">
        <div class="card-body">
            <h3 class="h5 fw-bold mb-4">
                <asp:Literal ID="lit_resumeTitle" runat="server" Text="<%$Resources:PnuInternetResources, res_Resume%>"></asp:Literal>
            </h3>

            <div class="row g-4">
                <div class="col-12">
                    <asp:TextBox ID="lblEmail" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-12">
                    <label class="form-label">
                        <asp:Literal ID="lit_scholar" runat="server" Text="<%$Resources:PnuInternetResources, res_ScholarId%>"></asp:Literal></label>
                    <asp:TextBox ID="txtScholarId" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-12">
                    <label class="form-label">
                        <asp:Literal ID="lit_orcid" runat="server" Text="<%$Resources:PnuInternetResources, res_ORCID%>"></asp:Literal></label>
                    <asp:TextBox ID="txtOrcid" placeholder="ORCID" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-12">
                    <label class="form-label">
                        <asp:Literal ID="lit_summary" runat="server" Text="<%$Resources:PnuInternetResources, res_Brief%>"></asp:Literal></label>
                    <asp:TextBox ID="txtSummary" TextMode="MultiLine" runat="server" Rows="3" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvfSummary" runat="server" Enabled="true" ErrorMessage="required" ValidationGroup="group1" ControlToValidate="txtSummary" CssClass="required" Style="color: red" Display="dynamic" />
                </div>

                <%-- الاهتمامات البحثية --%>
                <div class="col-12">
                    <label class="form-label">
                        <asp:Literal ID="lit_interests" runat="server"></asp:Literal></label>
                    <asp:TextBox ID="txtInterests" TextMode="MultiLine" runat="server" Rows="2" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </article>

    <%-- المؤهلات العلمية --%>
    <article class="card mb-4">
        <div class="card-body">
            <h3 class="h5 fw-bold mb-4">
                <asp:Literal ID="lit_degrees" runat="server"></asp:Literal>
            </h3>

            <div class="row g-4">
                <div class="col-12 col-md-4">
                    <label class="form-label">
                        <asp:Literal ID="lit_bachelor" runat="server"></asp:Literal></label>
                    <asp:TextBox ID="txtBachelor" TextMode="MultiLine" runat="server" Rows="2" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-12 col-md-4">
                    <label class="form-label">
                        <asp:Literal ID="lit_master" runat="server"></asp:Literal></label>
                    <asp:TextBox ID="txtMaster" TextMode="MultiLine" runat="server" Rows="2" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-12 col-md-4">
                    <label class="form-label">
                        <asp:Literal ID="lit_doctorate" runat="server"></asp:Literal></label>
                    <asp:TextBox ID="txtDoctorate" TextMode="MultiLine" runat="server" Rows="2" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </article>

    <%-- المناصب الحالية --%>
    <article class="card mb-4">
        <div class="card-body">
            <h3 class="h5 fw-bold mb-4">
                <asp:Literal runat="server" Text="<%$Resources:PnuInternetResources, res_CurrentPositions%>"></asp:Literal>
            </h3>

            <div class="row g-4">
                <div class="col-12 col-md-8">
                    <label class="form-label">
                        <asp:Literal ID="lit_curPos" runat="server" Text="<%$Resources:PnuInternetResources, res_CurrentPositions%>"></asp:Literal></label>
                    <asp:TextBox ID="txtCurPos" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Enabled="true" ErrorMessage="required" ValidationGroup="group2" ControlToValidate="txtCurPos" CssClass="required" Style="color: red" Display="dynamic" />
                </div>
                <div class="col-12 col-md-4">
                    <label class="form-label">
                        <asp:Literal ID="lit_sort" runat="server" Text="<%$Resources:PnuInternetResources, res_Sort%>"></asp:Literal></label>
                    <asp:TextBox ID="txtOrder" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Enabled="true" ErrorMessage="required" ValidationGroup="group2" ControlToValidate="txtOrder" CssClass="required" Style="color: red" Display="dynamic" />
                </div>
                <div class="col-12">
                    <asp:Button ID="btnPositions" CssClass="btn btn-secondary px-4" runat="server" Text="<%$Resources:PnuInternetResources, res_Add%>" ValidationGroup="group2" OnClick="btnPositions_Click" />
                </div>
                <div class="col-12">
                    <asp:Repeater ID="rep" runat="server" OnItemCommand="rep_ItemCommand">
                        <HeaderTemplate>
                            <div class="table-responsive">
                                <table class="table align-middle mb-0">
                                    <thead>
                                        <tr>
                                            <th scope="col" class="text-start ps-4">
                                                <asp:Literal ID="lit_rptitle" runat="server" Text="<%$Resources:PnuInternetResources, res_CurrentPositions%>"></asp:Literal></th>
                                            <th scope="col"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="text-start ps-4"><%# Eval("Title") %></td>
                                <td class="text-end pe-4">
                                    <asp:Button ID="btnDel" runat="server" CssClass="btn btn-outline-danger btn-sm px-3" Text="<%$Resources:PnuInternetResources, res_delete%>" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                    </tbody>
                                </table>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </article>

    <div class="d-flex justify-content-center">
        <asp:Button ID="btnSubmit" runat="server" ValidationGroup="group1" Text="<%$Resources:PnuInternetResources, res_Save%>" OnClick="btnSubmit_Click" CssClass="btn btn-primary px-5" />
    </div>

</div>
