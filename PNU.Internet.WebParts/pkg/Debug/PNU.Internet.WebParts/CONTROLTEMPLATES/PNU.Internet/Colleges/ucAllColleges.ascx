<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllColleges.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucAllColleges" %>





<style>
    img.d-block.w-100 
    {
    border-radius: 10px;
	height: 426px !important;
    
    }
</style>
<asp:UpdatePanel ID="updatepnl" runat="server" >  
<ContentTemplate> 
    <section>
      <div class="container py-5 mt-5">
        <div class="d-flex justify-content-center tabbable">
          <ul class="nav nav-tabs nav-pills  mb-5 bg-semi-light p-1 rounded-2" id="myTab" role="tablist">
            <li class="nav-item" role="presentation">
              <button class="nav-link active" id="home-tab" data-bs-toggle="tab" data-bs-target="#college1-tab-pane" type="button" role="tab" aria-controls="college1-tab-pane" aria-selected="true">الكليات
                الانسانية</button>
            </li>
            <li class="nav-item" role="presentation">
              <button class="nav-link" id="college2-tab" data-bs-toggle="tab" data-bs-target="#college2-tab-pane" type="button" role="tab" aria-controls="college2-tab-pane" aria-selected="false" tabindex="-1">الكليات
                العلمية</button>
            </li>
            <li class="nav-item" role="presentation">
              <button class="nav-link" id="college3-tab" data-bs-toggle="tab" data-bs-target="#college3-tab-pane" type="button" role="tab" aria-controls="college3-tab-pane" aria-selected="false" tabindex="-1">الكليات الصحية</button>
            </li>
            <li class="nav-item" role="presentation">
              <button class="nav-link" id="college4-tab" data-bs-toggle="tab" data-bs-target="#college4-tab-pane" type="button" role="tab" aria-controls="college4-tab-pane" aria-selected="false" tabindex="-1">العمادات والمعاهد</button>
            </li>
            <li class="nav-item" role="presentation">
              <button class="nav-link" id="college5-tab" data-bs-toggle="tab" data-bs-target="#college5-tab-pane" type="button" role="tab" aria-controls="college5-tab-pane" aria-selected="false" tabindex="-1">الكليات التطبيقية</button>
            </li>
          </ul>
        </div>
        <div class="tab-content" id="myTabContent">
          <div class="tab-pane fade active show" id="college1-tab-pane" role="tabpanel" aria-labelledby="college1-tab" tabindex="0">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">الكليات الانسانية
              <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
                        <div class="row">
                <asp:Repeater ID="rptColleges1" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-xl-4 col-xxl-3">
                            <div class="flip-card card item bg-transparent position-relative">
                                <a href="<%#DataBinder.Eval(Container.DataItem,"LinkUrl") %>" class="stretched-link "></a>
                                <div class="flip-card-inner">
                                    <div class="flip-card-front">
                                        <div class="card item bg-transparent rounded-4">
                                            <div class="thumb position-relative overflow-hidden">
                                                <img src="<%#DataBinder.Eval(Container.DataItem,"ImageUrl") %>" class="d-block  w-100">
                                            </div>
                                            <h2 class="position-absolute fixed-bottom text-white p-3 px-4">
                                                <%#DataBinder.Eval(Container.DataItem,"FacultyName") %>
                                            </h2>
                                            <h4 class="position-absolute fixed-top float-left text-white p-3 px-4 text-end">
                                                <span class="badge bg-warning"><%#DataBinder.Eval(Container.DataItem,"MainFaculty") %></span>
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="flip-card-back py-4 px-4 border">
                                        <h2 class="mb-3 text-black"><%#DataBinder.Eval(Container.DataItem,"FacultyName") %></h2>
                                        <p class="text-justify"><%#DataBinder.Eval(Container.DataItem,"Description") %></p>
                                        <div class="position-absolute fixed-bottom  text-end p-3 px-4">
                                            <img height="52" src="/ar/Faculties/PublishingImages/pnu-logo-en.svg" alt="pnu-logo" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
          </div>
          <div class="tab-pane fade" id="college2-tab-pane" role="tabpanel" aria-labelledby="college2-tab" tabindex="0">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">الكليات العلمية
              <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
                        <div class="row">
                <asp:Repeater ID="rptColleges2" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-xl-4 col-xxl-3">
                            <div class="flip-card card item bg-transparent position-relative">
                                <a href="<%#DataBinder.Eval(Container.DataItem,"LinkUrl") %>" class="stretched-link "></a>
                                <div class="flip-card-inner">
                                    <div class="flip-card-front">
                                        <div class="card item bg-transparent rounded-4">
                                            <div class="thumb position-relative overflow-hidden">
                                                <img src="<%#DataBinder.Eval(Container.DataItem,"ImageUrl") %>" class="d-block  w-100">
                                            </div>
                                            <h2 class="position-absolute fixed-bottom text-white p-3 px-4">
                                                <%#DataBinder.Eval(Container.DataItem,"FacultyName") %>
                                            </h2>
                                            <h4 class="position-absolute fixed-top float-left text-white p-3 px-4 text-end">
                                                <span class="badge bg-warning"><%#DataBinder.Eval(Container.DataItem,"MainFaculty") %></span>
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="flip-card-back py-4 px-4 border">
                                        <h2 class="mb-3 text-black"><%#DataBinder.Eval(Container.DataItem,"FacultyName") %></h2>
                                        <p class="text-justify"><%#DataBinder.Eval(Container.DataItem,"Description") %></p>
                                        <div class="position-absolute fixed-bottom  text-end p-3 px-4">
                                            <img height="52" src="/ar/Faculties/PublishingImages/pnu-logo-en.svg" alt="pnu-logo" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
          </div>
          <div class="tab-pane fade" id="college3-tab-pane" role="tabpanel" aria-labelledby="college3-tab" tabindex="0">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">الكليات الصحية
              <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
                        <div class="row">
                <asp:Repeater ID="rptColleges3" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-xl-4 col-xxl-3">
                            <div class="flip-card card item bg-transparent position-relative">
                                <a href="<%#DataBinder.Eval(Container.DataItem,"LinkUrl") %>" class="stretched-link "></a>
                                <div class="flip-card-inner">
                                    <div class="flip-card-front">
                                        <div class="card item bg-transparent rounded-4">
                                            <div class="thumb position-relative overflow-hidden">
                                                <img src="<%#DataBinder.Eval(Container.DataItem,"ImageUrl") %>" class="d-block  w-100">
                                            </div>
                                            <h2 class="position-absolute fixed-bottom text-white p-3 px-4">
                                                <%#DataBinder.Eval(Container.DataItem,"FacultyName") %>
                                            </h2>
                                            <h4 class="position-absolute fixed-top float-left text-white p-3 px-4 text-end">
                                                <span class="badge bg-warning"><%#DataBinder.Eval(Container.DataItem,"MainFaculty") %></span>
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="flip-card-back py-4 px-4 border">
                                        <h2 class="mb-3 text-black"><%#DataBinder.Eval(Container.DataItem,"FacultyName") %></h2>
                                        <p class="text-justify"><%#DataBinder.Eval(Container.DataItem,"Description") %></p>
                                        <div class="position-absolute fixed-bottom  text-end p-3 px-4">
                                            <img height="52" src="/ar/Faculties/PublishingImages/pnu-logo-en.svg" alt="pnu-logo" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
          </div>
          <div class="tab-pane fade" id="college4-tab-pane" role="tabpanel" aria-labelledby="college4-tab" tabindex="0">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">العمادات والمعاهد
              <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
                        <div class="row">
                <asp:Repeater ID="rptColleges4" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-xl-4 col-xxl-3">
                            <div class="flip-card card item bg-transparent position-relative">
                                <a href="<%#DataBinder.Eval(Container.DataItem,"LinkUrl") %>" class="stretched-link "></a>
                                <div class="flip-card-inner">
                                    <div class="flip-card-front">
                                        <div class="card item bg-transparent rounded-4">
                                            <div class="thumb position-relative overflow-hidden">
                                                <img src="<%#DataBinder.Eval(Container.DataItem,"ImageUrl") %>" class="d-block  w-100">
                                            </div>
                                            <h2 class="position-absolute fixed-bottom text-white p-3 px-4">
                                                <%#DataBinder.Eval(Container.DataItem,"FacultyName") %>
                                            </h2>
                                            <h4 class="position-absolute fixed-top float-left text-white p-3 px-4 text-end">
                                                <span class="badge bg-warning"><%#DataBinder.Eval(Container.DataItem,"MainFaculty") %></span>
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="flip-card-back py-4 px-4 border">
                                        <h2 class="mb-3 text-black"><%#DataBinder.Eval(Container.DataItem,"FacultyName") %></h2>
                                        <p class="text-justify"><%#DataBinder.Eval(Container.DataItem,"Description") %></p>
                                        <div class="position-absolute fixed-bottom  text-end p-3 px-4">
                                            <img height="52" src="/ar/Faculties/PublishingImages/pnu-logo-en.svg" alt="pnu-logo" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
          </div>
          <div class="tab-pane fade" id="college5-tab-pane" role="tabpanel" aria-labelledby="college5-tab" tabindex="0">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">الكليات التطبيقية
              <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
                        <div class="row">
                <asp:Repeater ID="rptColleges5" runat="server">
                    <ItemTemplate>
                        <div class="col-md-6 col-xl-4 col-xxl-3">
                            <div class="flip-card card item bg-transparent position-relative">
                                <a href="<%#DataBinder.Eval(Container.DataItem,"LinkUrl") %>" class="stretched-link "></a>
                                <div class="flip-card-inner">
                                    <div class="flip-card-front">
                                        <div class="card item bg-transparent rounded-4">
                                            <div class="thumb position-relative overflow-hidden">
                                                <img src="<%#DataBinder.Eval(Container.DataItem,"ImageUrl") %>" class="d-block  w-100">
                                            </div>
                                            <h2 class="position-absolute fixed-bottom text-white p-3 px-4">
                                                <%#DataBinder.Eval(Container.DataItem,"FacultyName") %>
                                            </h2>
                                            <h4 class="position-absolute fixed-top float-left text-white p-3 px-4 text-end">
                                                <span class="badge bg-warning"><%#DataBinder.Eval(Container.DataItem,"MainFaculty") %></span>
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="flip-card-back py-4 px-4 border">
                                        <h2 class="mb-3 text-black"><%#DataBinder.Eval(Container.DataItem,"FacultyName") %></h2>
                                        <p class="text-justify"><%#DataBinder.Eval(Container.DataItem,"Description") %></p>
                                        <div class="position-absolute fixed-bottom  text-end p-3 px-4">
                                            <img height="52" src="/ar/Faculties/PublishingImages/pnu-logo-en.svg" alt="pnu-logo" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
          </div>
        </div>
      </div>
    </section>





    </ContentTemplate>
    </asp:UpdatePanel>



	<script type="text/javascript">

        function activeTab(tabno) {
            $(document).ready(function () {
                $('#myTab button[data-bs-target="#college' + tabno + '-tab-pane"]').tab('show');
            });
        }

    </script>
