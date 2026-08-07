<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHomeStatistics.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage.DGA.ucHomeStatistics" %>






<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section class="py-5 numbers-section" data-aos="fade-up" aria-labelledby="numbers-section-title">
  <div class="container">
    <div class="mb-4">
      <div class="d-flex justify-content-between align-items-center gap-2">
        <h2 id="numbers-section-title" class="mb-0">
          <asp:Literal runat="server" Text="<%$ Resources: PNUres, PNUinNumbers %>" />
        </h2>
        <a class="btn btn-outline-secondary fw-semibold"
          href="/ar/AboutUniversity/Pages/PNUinNumbers.aspx">
          <asp:Literal runat="server" Text="<%$ Resources: CommonGlobalResources, ViewMore %>" />
        </a>
      </div>
    </div>

    <div class="g-4 row pt-2">
      <asp:Repeater ID="rptStatistics" runat="server">
        <ItemTemplate>
          <div class="col-6 col-lg-3 col-md-6">
            <div class="border-0 card rounded-0">
              <div class="align-items-center card-body">
                <div class="icon-container">
                  <span class="d-inline-flex">
                    <i class='<%# Eval("IconClass") %>' aria-hidden="true"></i>
                  </span>
                </div>
                <div class="text-center">
                  <h3 class="display-3 fw-normal text-nowrap" style="color:var(--dga-primary-800)">
                    <%# Eval("Count") %>
                  </h3>
                  <span style="color:var(--dga-gray-800)">
                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEn")) %>
                  </span>
                </div>
              </div>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>

  </div>
</section>




<%--
    <section class="py-5 numbers-section" data-aos="fade-up" aria-labelledby="numbers-section-title">
      <div class="container">
        <div class="mb-4">
          <div class="d-flex justify-content-between align-items-center gap-2">
            <h2 id="numbers-section-title" class="mb-0">الجامعة في أرقام</h2>
            <a class="btn btn-outline-secondary fw-semibold"
              href="/ar/AboutUniversity/Pages/PNUinNumbers.aspx">المزيد</a>
          </div>
        </div>
        <div class="g-4 row pt-2">
          <div class="col-6 col-lg-3 col-md-6">
            <div class="border-0 card rounded-0">
              <div class="align-items-center card-body">
                <div class="icon-container">
                  <span class="d-inline-flex">
                    <i class="hgi hgi-stroke hgi-school" aria-hidden="true"></i>
                  </span>
                </div>
                <div class="text-center">
                  <h3 class="display-3 fw-normal text-nowrap" style="color:var(--dga-primary-800)">18</h3>
                  <span style="color:var(--dga-gray-800)">الكليات والمعاهد</span>
                </div>
              </div>
            </div>
          </div>
          <div class="col-6 col-lg-3 col-md-6">
            <div class="border-0 card rounded-0">
              <div class="align-items-center card-body">
                <div class="icon-container">
                  <span class="d-inline-flex">
                    <i class="hgi hgi-stroke hgi-book-02" aria-hidden="true"></i>
                  </span>
                </div>
                <div class="text-center">
                  <h3 class="display-3 fw-normal text-nowrap" style="color:var(--dga-primary-800)">142</h3>
                  <span style="color:var(--dga-gray-800)">البرامج الأكاديمية</span>
                </div>
              </div>
            </div>
          </div>
          <div class="col-6 col-lg-3 col-md-6">
            <div class="border-0 card rounded-0">
              <div class="align-items-center card-body">
                <div class="icon-container">
                  <span class="d-inline-flex">
                    <i class="hgi hgi-stroke hgi-user-group" aria-hidden="true"></i>
                  </span>
                </div>
                <div class="text-center">
                  <h3 class="display-3 fw-normal text-nowrap" style="color:var(--dga-primary-800)">34428</h3>
                  <span style="color:var(--dga-gray-800)">طالبة</span>
                </div>
              </div>
            </div>
          </div>
          <div class="col-6 col-lg-3 col-md-6">
            <div class="border-0 card rounded-0">
              <div class="align-items-center card-body">
                <div class="icon-container">
                  <span class="d-inline-flex">
                    <i class="hgi hgi-stroke hgi-mentor" aria-hidden="true"></i>
                  </span>
                </div>
                <div class="text-center">
                  <h3 class="display-3 fw-normal text-nowrap" style="color:var(--dga-primary-800)">2142</h3>
                  <span style="color:var(--dga-gray-800)">أعضاء هيئة التدريس</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>--%>
