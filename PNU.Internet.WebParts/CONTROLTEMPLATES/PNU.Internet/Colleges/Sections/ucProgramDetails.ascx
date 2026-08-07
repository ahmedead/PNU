<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProgramDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Sections.ucProgramDetails" %>




    <section class="researcher-and-contributor ">
      <div class="container pt-5 my-5">
        
          <asp:Repeater ID="Repeater1" runat="server">
              <ItemTemplate>
			  <div class="pt-5 mt-5 d-flex justify-content-center">
          <div>
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5"><%#DataBinder.Eval(Container.DataItem,"Title") %>
              <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
            </h1>
          </div>
        </div>
		
	
					  <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4">
				  في سطرين
				  <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
				</h1>
                  <p class="text-muted h5 lh-base mb-5">
                      <%#DataBinder.Eval(Container.DataItem,"Overview") %>
                  </p>
              </ItemTemplate>
          </asp:Repeater>
        <div class="row pt-5">
          <div class="col-12 col-lg-6  order-lg-0 order-1 ">
            <div class="d-flex align-items-start mt-lg-5 pt-lg-5">
              <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4"> مرامنـــا
                <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
				<img class="d-none d-md-inline-block" src="/Style Library/NewStyle/UI5/images/coiled-arrow.png">
              </h1>
              
            </div>
            <div class="our-goals-numbers">
                <asp:Repeater ID="rptProgramGoals" runat="server">
                    <ItemTemplate>
                        <div class="mb-4">
                            <span><%#DataBinder.Eval(Container.DataItem,"DisplayNo") %></span> <%#DataBinder.Eval(Container.DataItem,"Title") %>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
          </div>
          <div class="col-12 col-lg-6 mt-4 pb-5  mb-5  text-start">
            <div class="faculty-dean-img position-relative">
                <asp:Repeater ID="rptPrograms" runat="server">
                    <ItemTemplate>
                            <img src="<%#DataBinder.Eval(Container.DataItem,"ImageUrl") %>" height="500"  alt="...">
                        </ItemTemplate>
                </asp:Repeater>
            </div>
          </div>

        </div>
      </div>
    </section>
	
	
    <section class="mb-5 pb-5">
      <div class="container">
        <div class="p-4 p-md-5 rounded-4 bg-resonant-blue-100">
          <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4">
            المخرجات
            <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
          </h1>
          

            <asp:Repeater ID="Repeater2" runat="server">
                <ItemTemplate>
                    <p class="text-muted h5 mb-3 lh-base">
                        <%#DataBinder.Eval(Container.DataItem,"Outputs") %>
                    </p>

                </ItemTemplate>
            </asp:Repeater>
        </div>
      </div>
    </section>
    <section class="mb-5 pb-5">
      <div class="container">
        <div class="accordion-card">
          <div class="accordion accordion-flush " id="accordionFlushExample">
            <div class="accordion-item  my-2 border-0">
              <h2 class="accordion-header" id="flush-headingOne">
                <button class="accordion-button collapsed border" type="button" data-bs-toggle="collapse"
                  data-bs-target="#flush-collapseOne" aria-expanded="false" aria-controls="flush-collapseOne">
                  شروط القبول والمتطلبات
                </button>
              </h2>
              <div id="flush-collapseOne" class="accordion-collapse collapse " aria-labelledby="flush-headingOne"
                data-bs-parent="#accordionFlushExample">

                <div class="accordion-body">
                  شروط القبول والمتطلبات
                </div>
              </div>
            </div>
            <div class="accordion-item  my-2 border-0">
              <h2 class="accordion-header" id="flush-headingTwo">
                <button class="accordion-button collapsed border" type="button" data-bs-toggle="collapse"
                  data-bs-target="#flush-collapseTwo" aria-expanded="false" aria-controls="flush-collapseTwo">
                  الرسوم
                </button>
              </h2>
              <div id="flush-collapseTwo" class="accordion-collapse collapse " aria-labelledby="flush-headingTwo"
                data-bs-parent="#accordionFlushExample">

                <div class="accordion-body">
                  الرسوم
                </div>
              </div>
            </div>
            <div class="accordion-item  my-2 border-0">
              <h2 class="accordion-header" id="flush-headingThree">
                <button class="accordion-button collapsed border" type="button" data-bs-toggle="collapse"
                  data-bs-target="#flush-collapseThree" aria-expanded="false" aria-controls="flush-collapseThree">
                  المواد الدراسية
                </button>
              </h2>
              <div id="flush-collapseThree" class="accordion-collapse collapse " aria-labelledby="flush-headingThree"
                data-bs-parent="#accordionFlushExample">

                <div class="accordion-body">
                  <div class="table-responsive">
                    <table class="table table-striped-secondary table-borderless fs-5 text-nowrap">
                      <thead>
                        <th>رمزه</th>
                        <th>اسم المقرر</th>
                        <th>مختصر التواصيف</th>
                      </thead>
                      <tbody>
                        <tr>
                          <td>منهج 263</td>
                          <td>العلوم الفيزيائية</td>
                          <td>مختصر التوصيف العربي</td>
                        </tr>
                        <tr>
                          <td>منهج 263</td>
                          <td>العلوم الفيزيائية</td>
                          <td>مختصر التوصيف العربي</td>
                        </tr>
                        <tr>
                          <td>منهج 263</td>
                          <td>العلوم الفيزيائية</td>
                          <td>مختصر التوصيف العربي</td>
                        </tr>
                        <tr>
                          <td>منهج 263</td>
                          <td>العلوم الفيزيائية</td>
                          <td>مختصر التوصيف العربي</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
    
    
 <style>
.breadcrumbhide {display: none;}
</style>   