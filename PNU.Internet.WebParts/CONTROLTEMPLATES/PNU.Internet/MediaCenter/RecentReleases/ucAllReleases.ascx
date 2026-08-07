<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllReleases.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.RecentReleases.ucAllReleases" %>




 <asp:UpdatePanel ID="updatepnl" runat="server" >  
<ContentTemplate> 

    <section class="py-5 mb-5">
      <div class="container">
        <form action="">
          <div class="d-flex flex-column flex-lg-row gap-3 p-2 p-lg-4 bg-light rounded-4 mb-5">
            <div class="input-group">
              <input type="text" class="form-control border-end-0" placeholder="" aria-label="بحث"
                aria-describedby="input-icon">
              <span class="input-group-text bg-white" id="input-icon">
                <svg width="20" height="20">
                  <use xlink:href="#searchIconGrey" />
                </svg>
              </span>
            </div>
            <select class="form-select">
              <option selected><asp:Literal ID="lit_classification" runat="server" Text="<%$Resources:PnuInternetResources, res_Classification%>"></asp:Literal> </option>
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
            </select>
            <select class="form-select">
              <option selected><asp:Literal ID="lit_faculty" runat="server" Text="<%$Resources:PnuInternetResources, res_Faculty%>"></asp:Literal> </option>
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
            </select>
            <input type="date" class="form-control flex-row-reverse text-start">
            <div class="flex-shrink-0">
              <button type="submit" class="btn btn-primary px-4"><asp:Literal ID="lit_Search" runat="server" Text="<%$Resources:PnuInternetResources, res_Search%>"></asp:Literal> </button>
              <button type="reset" class="btn btn-link text-decoration-none fw-bold"><asp:Literal ID="lit_wipeAll" runat="server" Text="<%$Resources:PnuInternetResources, res_wipeAll%>"></asp:Literal> </button>
            </div>
          </div>
        </form>
        <div class="row justify-content-between align-items-center my-4">
          <div class="col-sm-4 mb-2 mb-sm-0 fs-5 fw-bold"><asp:Literal ID="lit_Results" runat="server" Text="<%$Resources:PnuInternetResources, res_Results%>"></asp:Literal> </div>
          <div class="col-sm-4 col-lg-2">
            <select class="form-select">
              <option selected><asp:Literal ID="Lit_sort" runat="server" Text="<%$Resources:PnuInternetResources, res_Sort%>"></asp:Literal> </option>
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
            </select>
          </div>
        </div>
        <div class="row">
             <asp:Repeater ID="rptversions" runat="server">  
                        <ItemTemplate> 
          <div class="col-md-6 col-lg-4 col-xxl-3 mb-4">
            <div class="card border rounded-4 h-100">
              <div class="card-body p-2">
                <img src="<%#DataBinder.Eval(Container.DataItem," ImageUrl ") %>" class="object-fit-cover rounded-3 w-100" height="250">
                <div class="my-2 fs-4 fw-bold">
                    <a href="<%# String.Format("/ar/MediaCenter/RecentReleases/Pages/ReleaseDetails.aspx?ItemId={0}", Eval("ID")) %>" class="stretched-link text-decoration-none text-body strong">
                        <strong><%#DataBinder.Eval(Container.DataItem,"Title") %></strong>
                    </a>
                  
                </div>
                <div class="d-flex mb-3">
                  <svg width="19" height="20">
                    <use xlink:href="#calendar" />
                  </svg>
                  <span class="ms-2 text-muted"><%#DataBinder.Eval(Container.DataItem,"Date", "{0:yyyy/MMMM/dd}") %></span>
                </div>
                <p class="text-muted"><%#DataBinder.Eval(Container.DataItem,"Desc") %></p>
                <div class="mb-2">
                  <span
                            class="badge rounded-pill position-static text-bg-resonant-blue-100  text-secondary  me-1  ">
                            <%#DataBinder.Eval(Container.DataItem,"_x0054_ag1") %> </span>
                          <span
                            class="badge rounded-pill position-static text-bg-resonant-blue-100  text-secondary  me-1 ">
                            <%#DataBinder.Eval(Container.DataItem,"Tags2") %></span>
                        </span>
                </div>
              </div>
            </div>
          </div>
                                 </ItemTemplate>
                </asp:Repeater>
        </div>
        <nav class="mt-5" aria-label="Events navigation">
           <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">  
                <ItemTemplate>  
                    <asp:LinkButton ID="lnkPage"  
                        Style="padding: 8px; margin: 2px; background: lightgray; border: solid 1px #666; color: black; font-weight: bold"  
                        CommandName="Page" CommandArgument="<%# Container.DataItem %>" runat="server" Font-Bold="True"><%# Container.DataItem %></asp:LinkButton>  
                </ItemTemplate>  
            </asp:Repeater>  
        </nav>
      </div>
    </section>

     </ContentTemplate>
             <Triggers>
                    <%--<asp:AsyncPostBackTrigger ControlID="AllSectionsDDL" EventName="SelectedIndexChanged"  />--%>
                </Triggers>
        </asp:UpdatePanel>

