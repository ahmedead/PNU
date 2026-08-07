<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFacultyActivationReport.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ManageMemebers.ucFacultyActivationReport" %>



 
<main class="dga-main-body" tabindex="-1">
    <asp:HiddenField ID="hfCollege" runat="server" />
    <asp:HiddenField ID="hfMetric" runat="server" />
    <section class="py-5">
        <div class="container">
 
            <%-- Summary: activation per college --%>
            <article class="card mb-4">
                <div class="card-body">
                    <div class="d-flex flex-wrap justify-content-between align-items-center mb-4 gap-3">
                        <h3 class="h5 fw-bold mb-0">
                            <asp:Literal ID="ltrSummaryTitle" runat="server"></asp:Literal>
                        </h3>
                        <asp:Button ID="btnExportSummary" runat="server" CssClass="btn btn-secondary px-4"
                            OnClick="btnExportSummary_Click" />
                    </div>
 
                    <div class="table-responsive">
                        <table class="table align-middle mb-0">
                            <thead>
                                <tr>
                                    <th scope="col" class="text-start ps-4"><asp:Literal ID="ltrHdrCollege" runat="server"></asp:Literal></th>
                                    <th scope="col" class="text-center"><asp:Literal ID="ltrHdrTotal" runat="server"></asp:Literal></th>
                                    <th scope="col" class="text-center"><asp:Literal ID="ltrHdrActivated" runat="server"></asp:Literal></th>
                                    <th scope="col" class="text-center"><asp:Literal ID="ltrHdrPct" runat="server"></asp:Literal></th>
                                    <th scope="col" class="text-center">Google Scholar</th>
                                    <th scope="col" class="text-center">ORCID</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSummary" runat="server" OnItemCommand="rptSummary_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="text-start ps-4"><%# Eval("College") %></td>
                                            <td class="text-center">
                                                <asp:LinkButton ID="lbTotal" runat="server" CssClass="fw-bold" CommandName="Total" CommandArgument='<%# Eval("College") %>' Text='<%# Eval("Total") %>'></asp:LinkButton>
                                            </td>
                                            <td class="text-center">
                                                <asp:LinkButton ID="lbActivated" runat="server" CssClass="fw-bold" CommandName="Activated" CommandArgument='<%# Eval("College") %>' Text='<%# Eval("Activated") %>'></asp:LinkButton>
                                            </td>
                                            <td class="text-center">
                                                <span class="badge badge-info"><%# Eval("Pct") %>%</span>
                                            </td>
                                            <td class="text-center">
                                                <asp:LinkButton ID="lbScholar" runat="server" CssClass="fw-bold" CommandName="Scholar" CommandArgument='<%# Eval("College") %>' Text='<%# Eval("ScholarCount") %>'></asp:LinkButton>
                                            </td>
                                            <td class="text-center">
                                                <asp:LinkButton ID="lbOrcid" runat="server" CssClass="fw-bold" CommandName="Orcid" CommandArgument='<%# Eval("College") %>' Text='<%# Eval("OrcidCount") %>'></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                            <tfoot>
                                <tr class="fw-bold">
                                    <td class="text-start ps-4"><asp:Literal ID="ltrHdrGrandTotal" runat="server"></asp:Literal></td>
                                    <td class="text-center"><asp:Literal ID="ltrGrandTotal" runat="server"></asp:Literal></td>
                                    <td class="text-center"><asp:Literal ID="ltrGrandActivated" runat="server"></asp:Literal></td>
                                    <td class="text-center"><span class="badge badge-info"><asp:Literal ID="ltrGrandPct" runat="server"></asp:Literal>%</span></td>
                                    <td class="text-center"><asp:Literal ID="ltrGrandScholar" runat="server"></asp:Literal></td>
                                    <td class="text-center"><asp:Literal ID="ltrGrandOrcid" runat="server"></asp:Literal></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </article>
 
            <%-- Details (drill-down) --%>
            <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                <article class="card">
                    <div class="card-body">
                        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4 gap-3">
                            <h3 class="h5 fw-bold mb-0">
                                <asp:Literal ID="ltrDetailsTitle" runat="server"></asp:Literal>
                            </h3>
                            <div class="d-flex flex-wrap gap-2">
                                <asp:Button ID="btnExportDetails" runat="server" CssClass="btn btn-outline-secondary px-4"
                                    OnClick="btnExportDetails_Click" />
                                <asp:Button ID="btnExportAll" runat="server" CssClass="btn btn-secondary px-4"
                                    OnClick="btnExportAll_Click" />
                            </div>
                        </div>
 
                        <div class="table-responsive">
                            <table class="table align-middle mb-0">
                                <thead>
                                    <tr>
                                        <th scope="col" class="text-start ps-4">#</th>
                                        <th scope="col" class="text-start"><asp:Literal ID="ltrHdrName" runat="server"></asp:Literal></th>
                                        <th scope="col" class="text-start"><asp:Literal ID="ltrHdrEmail" runat="server"></asp:Literal></th>
                                        <th scope="col" class="text-start"><asp:Literal ID="ltrHdrSection" runat="server"></asp:Literal></th>
                                        <th scope="col" class="text-center"><asp:Literal ID="ltrHdrHasResume" runat="server"></asp:Literal></th>
                                        <th scope="col" class="text-center">Google Scholar</th>
                                        <th scope="col" class="text-center">ORCID</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptDetails" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="text-start ps-4"><%# Container.ItemIndex + 1 %></td>
                                                <td class="text-start"><%# Eval("Name") %></td>
                                                <td class="text-start"><a href='mailto:<%# Eval("Email") %>'><%# Eval("Email") %></a></td>
                                                <td class="text-start"><%# Eval("Section") %></td>
                                                <td class="text-center"><span class='<%# (bool)Eval("HasResume") ? "badge badge-success" : "badge badge-danger" %>'><%# Eval("HasResumeText") %></span></td>
                                                <td class="text-center"><%# Eval("ScholarText") %></td>
                                                <td class="text-center"><%# Eval("OrcidText") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </article>
            </asp:Panel>
 
        </div>
    </section>
</main>
 