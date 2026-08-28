<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucProgramsDetails.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.ucProgramsDetails" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>


    <section class="position-relative py-5">
      <div class="container">
        
        <!-- عنوان الفئة / البرامج -->
        <div class="d-flex align-items-center justify-content-between flex-wrap gap-3 mb-4 pb-3 border-bottom">
          <h1 class="h3 fw-bold text-dark mb-0 d-flex align-items-center gap-2">
            <i class="hgi hgi-stroke hgi-mortarboard-02 text-primary"></i>
            <asp:Label ID="lblscholarships" runat="server" Text=""></asp:Label>
          </h1>
        </div>

        <!-- شبكة بطاقات البرامج -->
        <div class="row g-4">
          <asp:Repeater ID="rptServices" runat="server">
            <ItemTemplate>
              <div class="col-12 col-md-6 col-lg-4">
                <div class="card h-100 border rounded-3 overflow-hidden d-flex flex-column text-start shadow-none">
                  <div class="position-relative overflow-hidden bg-light" style="height: 220px;">
                    <img src='<%# Eval("PublishingRollupImage") %>' class="w-100 h-100 object-fit-cover" alt='<%# Eval("Title") %>' loading="lazy" />
                  </div>
                  <div class="card-body d-flex flex-column p-4 flex-grow-1">
                    <h2 class="h5 fw-bold text-dark mb-2">
                      <a href='<%# Eval("URL") %>' class="text-dark text-decoration-none stretched-link">
                        <%# Eval("Title") %>
                      </a>
                    </h2>
                    <p class="text-muted small mb-4 flex-grow-1">
                      <%# Eval("Desc") %>
                    </p>
                    <div class="d-flex justify-content-between align-items-center pt-3 border-top mt-auto">
                      <span class="text-primary small fw-bold"><%# IsArabic ? "تفاصيل البرنامج" : "Program Details" %></span>
                      <span class="btn btn-sm btn-secondary rounded-circle d-inline-flex align-items-center justify-content-center p-2">
                        <i class='hgi hgi-stroke <%# IsArabic ? "hgi-arrow-left-02" : "hgi-arrow-right-02" %> fs-5'></i>
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
