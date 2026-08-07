<%@ Assembly Name="PNU.Workflow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d7c9a0875b8f4acc" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestsListFormUserControl.ascx.cs" Inherits="PNU.Workflow.WebParts.RequestsListForm.RequestsListFormUserControl" %>

<section class="breadcrumb">
    <div class="container py-5 my-3">
        <h1 class="display-6 fw-bold mb-3">قائمة الطلبات </h1>
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb h6">
                <li class="breadcrumb-item"><a href="#" class="text-decoration-none fw-bold">الرئيسية</a></li>
                <li class="breadcrumb-item"><a href="#" class="text-decoration-none fw-bold">الأخبار</a></li>
                <li class="breadcrumb-item active fw-bold text-turquoise-600" aria-current="page">قائمة الطلبات
                </li>
            </ol>
        </nav>

    </div>
</section>

<section>
    <div class="container pt-5 my-5">
        <h1 class="title text-dark fw-bold px-2  mb-4 mt-md-0 mt-4">قائمة الطلبات
        </h1>
        <div class="table-responsive">

            <asp:Repeater ID="rep" runat="server">


                <HeaderTemplate>
                    <table
                        class="table table-striped-secondary table-borderless fs-5 text-nowrap">
                        <thead>
                            <tr>
                                <th scope="col">العنوان</th>
                                <th scope="col">مقدم الطلب</th>
                                <th scope="col">تاريخ الطلب</th>
                                <th scope="col">حالة الطلب</th>
                                <th scope="col">عرض</th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr style="height: 25px;">
                        <td>
                            <%#Eval("Title").ToString()%>
                        </td>
                        <td>
                            <%#Eval("Requester").ToString()%>
                        </td>
                        <td>
                            <%#Eval("CreateDate").ToString()%>
                        </td>
                        <td>
                            <%#Eval("RequestStatus").ToString()%>
                        </td>
                        <td>
                            <a href="RequestDetails.aspx?RequestId=<%#Eval("RequestId").ToString()%>">عرض</a>
                        </td>


                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>



            </asp:Repeater>
        </div>
    </div>
</section>
