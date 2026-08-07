<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubmissionFormUserControl.ascx.cs" Inherits="PNU.Jobs.webparts.SubmissionForm.SubmissionFormUserControl" %>

  <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/swiper@8/swiper-bundle.min.js"></script>
  <script src="js/icons-svg.js"></script>

  <script src="js/main.js"></script>
  
   <link rel="stylesheet" href="css/styles.rtl.min.css" />
  <script src="js/color-modes.js"></script>
  <!-- <script type="module" src="js/icons-svg.js"></script> -->
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/swiper@8/swiper-bundle.min.css" />

<section class=" py-5 my-5">
    <div class="container">
        <div class="row">
            <div class="col-lg-5 mb-3 mb-lg-5 job-form">
                <div class="card mb-4 p-5 shadow border-0 rounded-4 ">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h4 class="card-title mb-4 fw-bold">نموذج التقديم
                    </h4>
                        </div>
                        <div class="col-md-6">
                            <div class="input-group mb-3">

                                <asp:TextBox ID="txtFName" class="form-control" runat="server" placeholder="الاسم الأول"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfFName" runat="server" Enabled="true" ErrorMessage="حقل مطلوب" ValidationGroup="group1" ControlToValidate="txtFName" CssClass="required" Style="color: red" Display="dynamic" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="input-group mb-3">

                                <asp:TextBox ID="txtLName" class="form-control" runat="server" placeholder="الاسم الأخير"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfLName" runat="server" Enabled="true" ErrorMessage="حقل مطلوب" ValidationGroup="group1" ControlToValidate="txtLName" CssClass="required" Style="color: red" Display="dynamic" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="input-group mb-3">

                                <asp:TextBox ID="txtEmail" class="form-control" runat="server" placeholder=" البريد الالكتروني  "></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfEmail" runat="server" Enabled="true" ErrorMessage="حقل مطلوب" ValidationGroup="group1" ControlToValidate="txtEmail" CssClass="required" Style="color: red" Display="dynamic" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" Enabled="true" ErrorMessage="صيغة الإيميل غير صحيحة" ValidationGroup="group1" ControlToValidate="txtEmail" CssClass="required" Style="color: red" Display="dynamic" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="input-group mb-3">

                                <asp:TextBox ID="txtPhone" class="form-control" MaxLength="14" runat="server" placeholder=" رقم الهاتف  "></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfPhone" runat="server" Enabled="true" ErrorMessage="حقل مطلوب" ValidationGroup="group1" ControlToValidate="txtPhone" CssClass="required" Style="color: red" Display="dynamic" />
                                <asp:RegularExpressionValidator ID="RevPhone" runat="server"
                                    ControlToValidate="txtPhone" ErrorMessage="صيغة الجوال غير صحيحة" ValidationGroup="group1"
                                    ValidationExpression="[0-9]{10}" CssClass="required" Style="color: red" Display="dynamic"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="col-md-12">

                            <asp:DropDownList ID="ddlNationality" runat="server" class="form-select  mb-3">
                                <asp:ListItem Text="الجنسية" Value="0" />
                                <asp:ListItem Text="سعودي" Value="سعودي" />
                                
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvNationality" runat="server" Enabled="true" ErrorMessage="حقل مطلوب" ValidationGroup="group1" ControlToValidate="ddlNationality" InitialValue="0" CssClass="required" Style="color: red" Display="dynamic" />
                        </div>
                        <div class="col-md-12">

                            <asp:DropDownList ID="ddlCountry" runat="server" class="form-select  mb-3">
                                 <asp:ListItem Text="الدولة" Value="0" />
                                <asp:ListItem Text="المملكة العربية السعودية" Value="المملكة العربية السعودية" />
                              
                            </asp:DropDownList>
                             <asp:RequiredFieldValidator ID="rvfCountry" runat="server" Enabled="true" ErrorMessage="حقل مطلوب" ValidationGroup="group1" ControlToValidate="ddlCountry" InitialValue="0" CssClass="required" Style="color: red" Display="dynamic" />
                        </div>
                        <div class="col-md-12">

                            <asp:TextBox ID="txtCity" class="form-control" runat="server" placeholder=" المدينة "  MaxLength="100"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="rvfCity" runat="server" Enabled="true" ErrorMessage="حقل مطلوب" ValidationGroup="group1" ControlToValidate="txtCity"  CssClass="required" Style="color: red" Display="dynamic" />
                        </div>
                        <div class="col-md-12 align-items-stretch text-center">

                            <asp:Button ID="btnSubmit" runat="server" Text="تقديم الطلب" OnClick="btnSubmit_Click" class="btn btn-lg btn-primary  px-5  me-3  flex-fill w-100" ValidationGroup="group1" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</section>
