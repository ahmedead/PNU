<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMediaFiles.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.MeidaFiles.ucMediaFiles" %>



<section class="page-padding">
    <div class="container">

<%--        <div class="d-flex justify-content-center justify-content-lg-start">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_MediaFiles %>" />
                <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
        </div>--%>

        <ul class="nav nav-tabs nav-underline nav-flush w-100 mb-4" id="myTab" role="tablist">
            <li class="nav-item" role="presentation">
                <button
                    class="nav-link border-top-0 border-end-0 border-start-0 active d-inline-flex align-items-center gap-2 bg-transparent px-2"
                    id="internal-tab" data-bs-toggle="tab" data-bs-target="#z1-tab-pane" type="button"
                    role="tab" aria-controls="z1-tab-pane" aria-selected="true">
                    <i class="hgi hgi-stroke hgi-calendar-03 fs-5 fw-light" aria-hidden="true"></i>
                    <span><asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_Weekly %>" /></span>
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button
                    class="nav-link border-top-0 border-end-0 border-start-0 d-inline-flex align-items-center gap-2 bg-transparent px-2"
                    id="external-tab" data-bs-toggle="tab" data-bs-target="#z2-tab-pane" type="button"
                    role="tab" aria-controls="z2-tab-pane" aria-selected="false" tabindex="-1">
                    <i class="hgi hgi-stroke hgi-calendar-02 fs-5 fw-light" aria-hidden="true"></i>
                    <span><asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_Monthly %>" /></span>
                </button>
            </li>
        </ul>

        <div class="tab-content pt-2" id="myTabContent">

            <!-- Weekly (أسبوعي) -->
            <div class="tab-pane fade show active" id="z1-tab-pane" role="tabpanel" aria-labelledby="internal-tab" tabindex="0">
                <asp:UpdatePanel ID="updatepnl" runat="server">
                    <ContentTemplate>
                        <div class="row g-4">
                            <asp:Repeater ID="rptWeekly" runat="server">
                                <ItemTemplate>
                                    <div class="col-12 col-lg-4 col-md-6">
                                        <article class="card h-100">
                                            <div class="card-body d-flex flex-column gap-4">
                                                <div class="icon-container">
                                                    <span class="d-inline-flex fs-3">
                                                        <i class="hgi hgi-stroke hgi-file-02 fs-3" aria-hidden="true"></i>
                                                    </span>
                                                </div>
                                                <div>
                                                    <h3 class="card-title"><%# Eval("FileName") %></h3>
                                                </div>
                                                <div class="d-flex gap-3 flex-wrap mt-auto">
                                                    <a role="button" class="btn btn-primary"
                                                        href='<%# Eval("FilePath") %>'
                                                        download='<%# Eval("FileName") %>'>
                                                        <asp:Literal ID="lit_download" runat="server"
                                                            Text="<%$Resources:PnuInternetResources, res_Download%>" />
                                                    </a>
                                                </div>
                                            </div>
                                        </article>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <nav class="mt-5">
                            <ul class="pagination justify-content-center gap-3">
                                <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                    <ItemTemplate>
                                        <li class="page-item">
                                            <asp:LinkButton ID="lnkPage" CssClass="page-link"
                                                CommandName="Page" CommandArgument="<%# Container.DataItem %>" runat="server"
                                                Font-Bold="True">
                                                <%# Container.DataItem %>
                                            </asp:LinkButton>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </nav>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Monthly (شهري) -->
            <div class="tab-pane fade" id="z2-tab-pane" role="tabpanel" aria-labelledby="external-tab" tabindex="0">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="row g-4">
                            <asp:Repeater ID="rptMonthly" runat="server">
                                <ItemTemplate>
                                    <div class="col-12 col-lg-4 col-md-6">
                                        <article class="card h-100">
                                            <div class="card-body d-flex flex-column gap-4">
                                                <div class="icon-container">
                                                    <span class="d-inline-flex fs-3">
                                                        <i class="hgi hgi-stroke hgi-file-02 fs-3" aria-hidden="true"></i>
                                                    </span>
                                                </div>
                                                <div>
                                                    <h3 class="card-title"><%# Eval("FileName") %></h3>
                                                </div>
                                                <div class="d-flex gap-3 flex-wrap mt-auto">
                                                    <a role="button" class="btn btn-primary"
                                                        href='<%# Eval("FilePath") %>'
                                                        download='<%# Eval("FileName") %>'>
                                                        <asp:Literal ID="lit_download" runat="server"
                                                            Text="<%$Resources:PnuInternetResources, res_Download%>" />
                                                    </a>
                                                </div>
                                            </div>
                                        </article>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <nav class="mt-5">
                            <ul class="pagination justify-content-center gap-3">
                                <asp:Repeater ID="Repeater2" runat="server" OnItemCommand="Repeater2_ItemCommand">
                                    <ItemTemplate>
                                        <li class="page-item">
                                            <asp:LinkButton ID="lnkPage" CssClass="page-link"
                                                CommandName="Page" CommandArgument="<%# Container.DataItem %>" runat="server"
                                                Font-Bold="True">
                                                <%# Container.DataItem %>
                                            </asp:LinkButton>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </nav>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
</section>

<script type="text/javascript">
    function activeTab(tabno) {
        $(document).ready(function () {
            $('#myTab button[data-bs-target="#z' + tabno + '-tab-pane"]').tab('show');
        });
    }
</script>
