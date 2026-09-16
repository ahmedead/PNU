<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSectionPrograms.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.ucSectionPrograms" %>


                    
  
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Panel ID="pnlData" runat="server">
    <!-- البرامج الأكاديمية للقسم -->
    <section class="container page-padding pt-0">
        <h3 class="h5 mt-4"><asp:Literal runat="server" Text="<%$ Resources: PNUres, AcademicPrograms %>" /></h3>

        <%-- Classification tabs (DGA underline nav) --%>
        <ul class="nav nav-tabs mt-3" id="myTab" role="tablist">
            <asp:Repeater ID="masterRepeater" runat="server">
                <ItemTemplate>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" id='<%# "new" + Eval("ID") + "-tab" %>' data-bs-toggle="tab" data-bs-target='<%# "#new" + Eval("ID") + "-tab-pane" %>'
                            type="button" role="tab" aria-controls='<%# "#new" + Eval("ID") + "-tab-pane" %>' aria-selected="false">
                            <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                        </button>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

        <div class="tab-content pt-3" id="myTabContent">
            <asp:Repeater ID="detailsRepeater" runat="server">
                <ItemTemplate>
                    <div class="tab-pane fade show active" id='<%# "new" + Eval("ID") + "-tab-pane" %>' role="tabpanel" aria-labelledby='<%# "new" + Eval("ID") + "-tab" %>' tabindex="0">
                        <div class="row g-3">
                            <asp:Repeater ID="rptPrograms" runat="server" DataSource='<%# Eval("Programs") %>'>
                                <ItemTemplate>
                                    <div class="col-12 col-md-6">
                                        <a class="card h-100 text-decoration-none" href="<%# String.Format("ProgramDetails.aspx?ProgramCode={0}", Eval("Code")) %>">
                                            <div class="card-body d-flex align-items-center gap-3">
                                                <span class="icon-container flex-shrink-0" aria-hidden="true">
                                                    <i class="hgi hgi-stroke hgi-diploma fs-3"></i>
                                                </span>
                                                <h4 class="h6 mb-0"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %></h4>
                                            </div>
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
    <!-- البرامج الأكاديمية للقسم -->
</asp:Panel>

<script type="text/javascript">

    function activeTab(tabno) {
        $(document).ready(function () {
            $('#myTab button[data-bs-target="#' + tabno + '-tab-pane"]').tab('show');
        });
    }

</script>
