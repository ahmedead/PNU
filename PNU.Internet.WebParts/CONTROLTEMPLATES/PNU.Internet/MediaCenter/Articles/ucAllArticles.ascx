<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllArticles.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.Articles.ucAllArticles" %>


<section class="py-5 mb-5">
  <div class="container">
    <div class="row">
      <asp:Repeater ID="rptNews" runat="server">
        <ItemTemplate>
          <div class="col-md-6 col-lg-4 mb-4">
            <div class="card border rounded-4 h-100">
              <div class="card-body p-2">
                <img src="<%#DataBinder.Eval(Container.DataItem," ImageUrl") %>" class="object-fit-cover rounded-3 w-100" height="250"
               >
                <div class="my-2 fs-4 fw-bold">
                  <asp:HyperLink runat="server" ID="link2" Target="_blank">
                    <h5 class="card-title mb-3 fw-bold">
                      <asp:Label ID="lblNewsTitle2" runat="server" Text=" "></asp:Label>
                    </h5>
                  </asp:HyperLink>
                  <a href="<%#DataBinder.Eval(Container.DataItem," Link") %>" class="stretched-link
                    text-decoration-none text-body strong"><%#DataBinder.Eval(Container.DataItem,"Title") %></a>
                </div>
                <div class="d-flex mb-3">
                  <svg width="19" height="20">
                    <use xlink:href="#calendar" />
                  </svg>
                  <span class="ms-2 text-muted">
                    <%#DataBinder.Eval(Container.DataItem,"Date") %>
                  </span>
                </div>

                <div class="mb-2">
                  <span class="badge rounded-pill bg-resonant-blue-100 text-secondary">
                    <%#DataBinder.Eval(Container.DataItem,"ArticleEditor") %>
                  </span><br />
                     <span class="badge rounded-pill bg-resonant-blue-100 text-secondary">
                    <%#DataBinder.Eval(Container.DataItem,"Degree") %>

                  </span>
                </div>
              </div>
            </div>
          </div>
        </ItemTemplate>
      </asp:Repeater>
    </div>
    <nav class="mt-5">
      <ul class="pagination justify-content-center gap-3">
        <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand" >
          <ItemTemplate>
            <li class="page-item">
                <asp:LinkButton ID="lnkPage"     CssClass="page-link" CommandName="Page" CommandArgument="<%# Container.DataItem %>" runat="server" Font-Bold="True">
                  <%# Container.DataItem %>
                </asp:LinkButton>
            </li>
          </ItemTemplate>
        </asp:Repeater>
      </ul>
    </nav>
  </div>
</section>
