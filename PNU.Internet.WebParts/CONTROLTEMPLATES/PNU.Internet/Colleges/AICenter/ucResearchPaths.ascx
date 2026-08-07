<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucResearchPaths.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucResearchPaths" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>







<style>
    img.d-block.w-100 
    {
    border-radius: 10px;
	height: 426px !important;
    
    }
</style>

<div id="div1" runat="server">
    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">
        <asp:Literal runat="server" Text="<%$ Resources: PNUres, ResearchPaths %>" />
        <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
    </h1>

</div>
<div id="div2" runat="server">
    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">
        <asp:Literal runat="server" ID="ltr2" Text="<%$ Resources: PNUres, ResearchPaths %>" />
        <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
    </h1>

</div>

<div id="div3" runat="server">
    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">
        <asp:Literal runat="server" ID="ltr3" Text="<%$ Resources: PNUres, ResearchPaths %>" />
        <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
    </h1>

</div>
 <div class="row">
     <asp:Repeater ID="rptCourses" runat="server" >
         <ItemTemplate>
             <div class="col-md-6 col-xl-4 col-xxl-3">
                 <div class="flip-card card item bg-transparent position-relative">
                     <a href="<%#DataBinder.Eval(Container.DataItem,"LinkUrl") %>" class="stretched-link "></a>
                     <div class="flip-card-inner">
                         <div class="flip-card-front">
                             <div class="card item bg-transparent rounded-4 border-0">
                                 <div class="thumb position-relative overflow-hidden">
                                     <img src="<%#DataBinder.Eval(Container.DataItem,"ImageUrl") %>" class="d-block  w-100">
                                 </div>
                                 <%--<h2 class="position-absolute fixed-bottom text-white p-3 px-4">
                                     <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title")) %>
                                 </h2>--%>

                             </div>
                         </div>
                         <div class="flip-card-back py-4 px-4 border">
                             <h2 class="mb-3 text-black">
                                 <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title")) %>
                             </h2>
                             <p class="text-justify"  id='<%# "paragraph_" + Container.ItemIndex %>'>
                                 <%# SPFactory.GetLocalizedTitle(Eval("DescriptionDisplay"), Eval("DescriptionDisplay")) %>
                             </p>
                             
                         </div>
                     </div>
                 </div>
             </div>
         </ItemTemplate>
     </asp:Repeater>
 </div>





<script type="text/javascript">

    function activeTab(tabno) {
        $(document).ready(function () {
            $('#myTab button[data-bs-target="#news' + tabno + '-tab-pane"]').tab('show');
        });
    }

</script>
