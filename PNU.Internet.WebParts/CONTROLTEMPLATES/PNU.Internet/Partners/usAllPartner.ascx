<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="usAllPartner.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Partners.usAllPartner" %>


<section class="py-5 mb-5">
    <div class="container">
        <div class="d-lg-flex justify-content-between   ">
            <div class="d-flex justify-content-center">
                <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                    <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_PnuPartnerTitle %>" />
                    <span class="px-2 position-absolute mt-1 h2 text-primary">•</span> </h1>
            </div>
            <div class="text-center mb-0">
                <div class="d-flex justify-content-center">

                    <ul class="nav  nav-pills flex-nowrap  bg-semi-light p-1 rounded-2" id="myTab" role="tablist">

                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="internal-tab" data-bs-toggle="tab" data-bs-target="#z1-tab-pane" type="button" role="tab" aria-controls="internal-tab-pane" aria-selected="true">
                                <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_LocalPartner %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="external-tab" data-bs-toggle="tab" data-bs-target="#z2-tab-pane" type="button" role="tab" aria-controls="external-tab-pane" aria-selected="false" tabindex="-1">
                                <asp:Literal runat="server" Text="<%$ Resources: PnuInternetResources, res_InternationalPartner %>" /></button>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
      <div class="tab-content">
        <div class="tab-pane fade show active" id="z1-tab-pane" role="tabpanel" aria-labelledby="1-tab" tabindex="0">
            <asp:UpdatePanel ID="updatepnl" runat="server">
                <ContentTemplate>
                    <div class="d-flex flex-wrap justify-content-between">
                        <asp:Repeater ID="rptLocal" runat="server">
                            <ItemTemplate>

                                <div class="m-3 d-flex" >

                                    <img src="<%# Eval("Logo") %>"  height="80px">
                                </div>

                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <nav class="mt-5" aria-label="Partners navigation">
                        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPage"
                                    Style="padding: 8px; margin: 2px; background: lightgray; border: solid 1px #666; color: black; font-weight: bold"
                                    CommandName="Page" CommandArgument="<%# Container.DataItem %>" runat="server" Font-Bold="True"><%# Container.DataItem %>  
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </nav>
                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
        <div class="tab-pane fade" id="z2-tab-pane" role="tabpanel" aria-labelledby="2-tab" tabindex="1">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="d-flex flex-wrap justify-content-between">
                        <asp:Repeater ID="rptInternational" runat="server">
                            <ItemTemplate>

                                 <div class="m-3 d-flex" >

                                    <img src="<%# Eval("Logo") %>"  height="80px">
                                </div>

                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <nav class="mt-5" aria-label="Partners navigation">
                        <asp:Repeater ID="Repeater2" runat="server" OnItemCommand="Repeater2_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPage"
                                    Style="padding: 8px; margin: 2px; background: lightgray; border: solid 1px #666; color: black; font-weight: bold"
                                    CommandName="Page" CommandArgument="<%# Container.DataItem %>" runat="server" Font-Bold="True"><%# Container.DataItem %>  
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
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
            $('#myTab button[data-bs-target="#' + tabno + '-tab-pane"]').tab('show');
        });
    }

</script>
