<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobsListUserControl.ascx.cs" Inherits="PNU.Jobs.webparts.JobsList.JobsListUserControl" %>


<style>
.card.border.rounded-4.h-100 {
    width: 400px;
}
</style>

<section class="py-5 mb-5">
    <div class="container">

        <div class="d-flex flex-column flex-lg-row gap-3 p-2 p-lg-4 bg-light rounded-4 mb-5">
            <div class="input-group">
               
                <asp:TextBox ID="txtSearchInputs" runat="server" class="form-control border-end-0" placeholder="بحث" aria-label="بحث"
                    aria-describedby="input-icon"></asp:TextBox>
                <span class="input-group-text bg-white" id="input-icon">
                    <svg width="20" height="20">
                        <use xlink:href="#searchIconGrey" />
                    </svg>
                </span>
            </div>
         
              <asp:DropDownList ID="ddlJobType" runat="server" class="form-select">
                </asp:DropDownList>
         
              <asp:DropDownList ID="ddlJobCategory" runat="server" class="form-select">
                   
                </asp:DropDownList>

            <%--<input type="date" class="form-control flex-row-reverse text-start">--%>
            <asp:TextBox ID="txtDate" runat="server" TextMode="Date" class="form-control flex-row-reverse text-start"></asp:TextBox>
            <div class="flex-shrink-0">
             
                <asp:Button ID="btnSearch" runat="server" Text="بحث" class="btn btn-primary px-4"  OnClick="btnSearch_Click"/>
                 <asp:Button ID="btnClear" runat="server" Text="مسح الكل" class="btn btn-primary px-4" OnClick="btnClear_Click" />
            </div>
        </div>
        <div class="row justify-content-between align-items-center my-4">
          <%--  <div class="col-sm-4 mb-2 mb-sm-0 fs-5 fw-bold text-muted">عرض نتائج 1-9 من 23</div>--%>
            <div class="col-sm-4 col-lg-2">
              
                <asp:DropDownList ID="ddlSorting" runat="server" class="form-select" OnSelectedIndexChanged="ddlSorting_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="ترتيب" Value="0"></asp:ListItem>
                    <asp:ListItem Text="تصاعدي" Value="True"></asp:ListItem>
                    <asp:ListItem Text="تنازلي" Value="False"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <asp:Repeater ID="rptgetAllData" runat="server">
                <HeaderTemplate>
                    <table>
                        <tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <td>

                        <div class="col-md-6 col-lg-4 col-xxl-3 mb-4">
                            <div class="card border rounded-4 h-100">
                                <div class="card-body p-2">
                                    <div class="my-2 fs-4 fw-bold">
                                        <a href="JobDetails.aspx?jobId=<%#Eval("UniqueId")%>" class="stretched-link text-decoration-none text-body fw-bolder"><%#Eval("JobTitle")%></a>
                                    </div>
                                    <div class="d-flex mb-3">
                                        <svg width="19" height="20">
                                            <use xlink:href="#calendar" />
                                        </svg>
                                        <span class="ms-2 text-muted"><%#((DateTime)Eval("JobDate")).ToString("dd/MM/yyyy")%></span>
                                    </div>
                                    <p class="text-muted"><%#Eval("JobDescription")%></p>
                                    <div class="mb-2">
                                        <span class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#Eval("JobCategory")%></span>
                                        <span class="badge rounded-pill bg-resonant-blue-100 text-secondary"><%#Eval("JobType")%></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </ItemTemplate>
                <FooterTemplate>
                    </tr>
                </table>
                </FooterTemplate>
            </asp:Repeater>

        </div>

        <div>
            <asp:Repeater ID="rptPaging" runat="server" OnItemCommand="rptPaging_ItemCommand">
                <HeaderTemplate>
                    <nav class="my-5" aria-label="Events navigation">
                        <ul class="pagination justify-content-center gap-3">
                </HeaderTemplate>
                <ItemTemplate>
                    <li class="page-item active">
                        <asp:LinkButton ID="btnPage" class="page-link"
                            CommandName="Page" CommandArgument="<%# Container.DataItem %>"
                            runat="server" ForeColor="White" Font-Bold="True"><%# Container.DataItem %>
                        </asp:LinkButton>

                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
         </nav>
                </FooterTemplate>
            </asp:Repeater>

        </div>

    </div>
</section>
