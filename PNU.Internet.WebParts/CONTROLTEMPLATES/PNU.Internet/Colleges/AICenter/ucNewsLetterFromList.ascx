<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewsLetterFromList.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucNewsLetterFromList" %>


<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>





<%
    // Check the current site language
    var currentLanguage = SPContext.Current.Web.Language;
    bool isArabic = (currentLanguage == 1025);
    string mainTitle = isArabic ? "النشرات الدورية" : "Newsletters";
    string openTitle = isArabic ? "عرض النشرة" : "Open Newsletter";
%>

<div class="the-message p-2">
  <div class="mt-3 px-md-5 px-0">
    <div class="d-block">
      <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
        <%= mainTitle %>
      </h1>
    </div>
  </div>

  <!-- Cards -->
  <div class="row g-3">
    <asp:Repeater ID="rptNewsletters" runat="server">
      <ItemTemplate>
        <div class="col-md-6">
          <div class="card h-100">
            <div class="card-header text-center fw-bold">
              <%#: Eval("DisplayTitle") %>
            </div>
            <div class="card-body text-center">
              <iframe allowfullscreen class="fp-iframe"
                      src="<%#: Eval("FlipUrl") %>"
                      style="border:1px solid lightgray;width:100%;height:400px;"></iframe>
              <button type="button"
                      class="btn btn-primary mt-3"
                      data-bs-toggle="modal"
                      data-bs-target="#flipbookModal_<%#: Eval("ModalId") %>">
                <asp:Literal runat="server" Text="<%$ Resources: PNUres, openTitle %>" />
              </button>
            </div>
          </div>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>

  <!-- Modals -->
  <asp:Repeater ID="rptModals" runat="server">
    <ItemTemplate>
      <div class="modal fade"
           id="flipbookModal_<%#: Eval("ModalId") %>"
           tabindex="-1"
           aria-hidden="true">
        <div class="modal-dialog modal-xl">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title"><%#: Eval("DisplayTitle") %></h5>
              <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body text-center">
              <iframe allowfullscreen class="fp-iframe"
                      src="<%#: Eval("FlipUrl") %>"
                      style="border:1px solid lightgray;width:100%;height:800px;"></iframe>
            </div>
          </div>
        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>
</div>
               