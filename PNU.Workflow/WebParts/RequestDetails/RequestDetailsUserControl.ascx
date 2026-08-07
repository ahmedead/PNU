<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestDetailsUserControl.ascx.cs" Inherits="PNU.Workflow.WebParts.RequestDetails.RequestDetailsUserControl" %>


<section class=" pt-5 mt-5 mb-4">
    <div class="container">
        <div class="row justify-content-center">

            <div class="col-lg-7 mb-4 mb-lg-5 news-form">
                <div class="card mb-4 p-5 shadow border-0 rounded-4 ">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="mb-4">

                                <label>المجموعة الرئيسية</label>
                                <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" Enabled="false"> </asp:TextBox>

                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="mb-4">

                                <label>نوع الخبر</label>
                                <asp:TextBox ID="txtMediaTypes" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>

                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="mb-4">

                                <label>التاريخ</label>
                                <asp:TextBox ID="txtDate" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>


                            </div>

                        </div>
                        <div class="col-md-12">
                            <div class="mb-4">

                                <label>الجهة الطالبة</label>
                                <asp:Label ID="lblFacultyName" runat="server"></asp:Label>

                            </div>

                        </div>


                        <div class="col-md-12">
                            <div class=" mb-4">
                                <label>عنوان الخبر</label>
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class=" mb-4">
                                <label>عنوان الخبر انجليزي</label>
                                <asp:TextBox ID="txtTitleEn" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class=" mb-4">
                                <label>صورة الخبر</label>
                                <asp:Image ID="Image1" runat="server" CssClass="img-fluid" />

                            </div>
                        </div>


                        <div class="col-md-12 d-none">
                            <div class="mb-4">
                                <label>Summary</label>
                                <asp:TextBox ID="txtSummary" TextMode="MultiLine" runat="server" Rows="3" CssClass="form-control"
                                    Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12 d-none">
                            <div class="mb-4">
                                <label>Summary English</label>
                                <asp:TextBox ID="txtSummaryEn" TextMode="MultiLine" runat="server" Rows="3" CssClass="form-control"
                                    Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="mb-4">
                                <label>محتوى الخبر </label>
                                <asp:TextBox ID="txtDetails" TextMode="MultiLine" runat="server" Rows="30" CssClass="form-control"
                                    Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="mb-4">
                                <label>محتوى الخبرانجليزي</label>
                                <asp:TextBox ID="txtDetailsEn" TextMode="MultiLine" runat="server" Rows="30" CssClass="form-control"
                                    Enabled="false">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="form-floating mb-4">
                                <asp:Label ID="Label7" runat="server" Text="Video URL"></asp:Label>
                                <asp:TextBox ID="txtVideoURL" runat="server" class="form-control" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</section>
