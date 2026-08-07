<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegePrograms.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucCollegePrograms" %>


                    

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>


<style>
    img.img-fluid {
    height: 180px;
}
</style>

<asp:Panel ID="pnlData" runat="server">
 <!-- البرامج الأكاديمية للقسم -->
<section class="also-know bg-transparent position-relative">
    <div class="container py-5 ">
        <div class="d-flex align-items-start  mb-4 mt-5">
            <h1
                class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-5 me-md-4 flex-shrink-0"><asp:Literal runat="server" Text="<%$ Resources: PNUres, AcademicProgramsCollege %>" /> 
                        <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
        </div>
        <div class="d-flex justify-content-between align-items-center flex-wrap flex-lg-nowrap mb-5">

    <div class="col-12 col-lg-auto tabbable">
        <ul class="nav nav-tabs nav-pills bg-semi-light p-1 rounded-2" id="myTab" role="tablist">

            <asp:Repeater ID="masterRepeater" runat="server">
                <ItemTemplate>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id='<%# "new" + Eval("ID") + "-tab" %>' data-bs-toggle="tab" data-bs-target='<%# "#new" + Eval("ID") + "-tab-pane" %>'
                            type="button" role="tab" aria-controls='<%# "#new" + Eval("ID") + "-tab-pane" %>' aria-selected="false">
                            <%# Eval("Title").ToString() %>
                        </button>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</div>

<div class="row">
    <div class="tab-content" id="myTabContent">
        <asp:Repeater ID="detailsRepeater" runat="server">
            <ItemTemplate>
                <div class="tab-pane fade" id='<%# "new" + Eval("ID") + "-tab-pane" %>' role="tabpanel" aria-labelledby='<%# "new" + Eval("ID") + "-tab" %>' tabindex="0">
                    <div class="row">
                        <div class="row my-4 pb-5">
                            <asp:Repeater ID="rptPrograms" runat="server" DataSource='<%# Eval("Programs") %>'>
                                <ItemTemplate>
                                    <div class="col-lg-4 col-md-12 ">
                                        <a href="<%# String.Format("ProgramDetails.aspx?ProgramCode={0}", Eval("Code")) %>">
                                            <div class="card mb-4 p-0  also-know-card">
                                                <img src="<%#DataBinder.Eval(Container.DataItem,"DisplayImage") %>" class="img-fluid " alt="...">
                                                <div class="position-absolute bottom-0 text-center end-0 start-0">
                                                    <h2 class="card-title h2 mb-4 pb-2 fw-bold text-white ">
                                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                </div>
                                            </div>
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>
</div>
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
    
