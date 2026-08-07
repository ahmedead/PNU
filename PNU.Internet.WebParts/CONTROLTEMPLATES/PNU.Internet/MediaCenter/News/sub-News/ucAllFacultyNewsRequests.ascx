<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllFacultyNewsRequests.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.sub_News.FacultyNewsRequests" %>


<style>


.fcc-btn {
  
  color: white !important;
  padding: 15px 25px 15px !important;
}

</style>
<section class="breadcrumb">
    <div class="container py-5 my-3">
        <h1 class="display-6 fw-bold mb-3">قائمة الطلبات </h1>
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb h6">
                <li class="breadcrumb-item"><a href="#" class="text-decoration-none fw-bold">الرئيسية</a></li>
                <li class="breadcrumb-item"><a href="#" class="text-decoration-none fw-bold">أخبار الكليات</a></li>
                <li class="breadcrumb-item active fw-bold text-turquoise-600" aria-current="page">قائمة الطلبات
                </li>
            </ol>
        </nav>

    </div>
</section>

<section>
    <div class="container pt-5 my-5">

          <div  id="dvAddNew"  runat="server" visible="false" >
            <a  class="btn btn-primary fcc-btn"  href="addFacultyNews.aspx">إضافة خبر جديد</a>
        </div>

        <h1 id="hMsg" runat="server" class="title text-dark fw-bold px-2  mb-4 mt-md-0 mt-4">قائمة الطلبات
        </h1>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="table-responsive" id="dvForm" runat="server">
                    <table class="table table-striped-secondary table-borderless fs-5">
                        <thead>
                            <tr>
                                <th scope="col">العنوان</th>
                                <th scope="col">الجهة الطالبة</th>
                                <th scope="col">مقدم الطلب</th>
                                <th scope="col">تاريخ الطلب</th>
                                <th scope="col">حالة الطلب</th>
                                <th scope="col">عرض</th>
                            </tr>
                        </thead>

                        <asp:Repeater ID="rep" runat="server">

                            <ItemTemplate>
                                <tr style="height: 25px;">
                                    <td>
                                        <%#Eval("Title").ToString()%>
                                    </td>
                                    <td class="text-nowrap">
                                        <%#Eval("FacultyName").ToString()%>
                                    </td>
                                    <td class="text-nowrap">
                                        <%#Eval("RequesterNameDisplay").ToString()%>
                                    </td>
                                    <td class="text-nowrap">
                                        <%#Eval("Created").ToString()%>
                                    </td>
                                    <td class="text-nowrap">
                                        <%#Eval("RequestStatus").ToString()%>
                                    </td>
                                    <td>
                                        <a href="RequestDetails.aspx?RequestId=<%#Eval("ID").ToString()%>">عرض</a>
                                    </td>


                                </tr>
                            </ItemTemplate>

                        </asp:Repeater>
                    </table>

                    <nav class="mt-5">
                        <ul class="pagination justify-content-center gap-3">
                            <asp:Repeater ID="Repeater1" runat="server" OnItemCommand="Repeater1_ItemCommand">
                                <ItemTemplate>
                                    <li class="page-item">
                                        <asp:LinkButton ID="lnkPage" CssClass="page-link" 
                                            CommandName="Page" CommandArgument="<%# Container.DataItem %>" runat="server"
                                            Font-Bold="True">
                                        <%# Container.DataItem %>
                                        </asp:LinkButton>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </nav>
                </div>
            </ContentTemplate>

        </asp:UpdatePanel>
    </div>

</section>
