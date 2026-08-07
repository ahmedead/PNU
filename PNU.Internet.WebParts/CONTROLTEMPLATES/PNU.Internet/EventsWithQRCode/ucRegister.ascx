<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRegister.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EventsWithQRCode.ucRegister" %>



<section class=" pt-5 mt-5 mb-4">
    <div class="container">
        <div class="row justify-content-center">

            <div class="col-lg-7 mb-4 mb-lg-5 news-form">

                <div id="divAlerts" visible="false" runat="server" class="alert alert-warning" style="color: black!important;">
    <div>
        <asp:Label ID="lblerr" ForeColor="Red" runat="server" Text=""></asp:Label>
    </div>
</div>


                <div class="card mb-4 p-5 shadow border-0 rounded-4 " id="DivRegister" runat="server">
                    <div class="row">
                        <div class="col-md-12 text-start">
                            <h4 class="card-title mb-4 fw-bold">نموذج التسجيل في فعالية كرنفال المهنة</h4>
                        </div>

                        <div class="row g-3">
                            <div class="col-md-12">
                                <div class="form-floating mb-1">
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="الاسم الرباعي"></asp:TextBox>
                                    <label for="<%= txtFullName.ClientID %>">الاسم الرباعي</label>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                    ErrorMessage="يرجى إدخال الاسم الرباعي" ForeColor="Red" Display="Dynamic" FontSize="Small"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6">
                                <div class="form-floating mb-1">
                                    <asp:TextBox ID="txtIDNumber" runat="server" CssClass="form-control" placeholder="رقم الهوية"></asp:TextBox>
                                    <label for="<%= txtIDNumber.ClientID %>">رقم الهوية أو الرقم الجامعي</label>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvID" runat="server" ControlToValidate="txtIDNumber"
                                    ErrorMessage="هذا الحقل مطلوب" ForeColor="Red" Display="Dynamic" FontSize="Small"></asp:RequiredFieldValidator>

                                <asp:RegularExpressionValidator ID="revID" runat="server" ControlToValidate="txtIDNumber"
                                    ErrorMessage="يجب أن يتكون رقم الهوية من 10 أرقام ويبدأ بـ 1 أو 2" ForeColor="Red" Display="Dynamic" FontSize="Small"
                                    ValidationExpression="^[12]\d{9}$"></asp:RegularExpressionValidator>
                            </div>

                            <div class="col-md-6">
                                <div class="form-floating mb-1">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="name@example.com"></asp:TextBox>
                                    <label for="<%= txtEmail.ClientID %>">البريد الإلكتروني</label>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="البريد الإلكتروني مطلوب" ForeColor="Red" Display="Dynamic" FontSize="Small"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="صيغة البريد الإلكتروني غير صحيحة" ForeColor="Red" Display="Dynamic" FontSize="Small"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>

                            <div class="col-md-6">
                                <div class="form-floating mb-1">
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="05xxxxxxxx"></asp:TextBox>
                                    <label for="<%= txtPhone.ClientID %>">رقم الجوال</label>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                                    ErrorMessage="رقم الجوال مطلوب" ForeColor="Red" Display="Dynamic" FontSize="Small"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                                    ErrorMessage="يجب أن يبدأ بـ 05 ويتكون من 10 أرقام" ForeColor="Red" Display="Dynamic" FontSize="Small"
                                    ValidationExpression="^(05)\d{8}$"></asp:RegularExpressionValidator>
                            </div>

                            <div class="col-md-6">
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtCollege" runat="server" CssClass="form-control" placeholder="الكلية"></asp:TextBox>
                                    <label for="<%= txtCollege.ClientID %>">الكلية (اختياري)</label>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="form-floating mb-1">
                                    <asp:TextBox ID="txtMajor" runat="server" CssClass="form-control" placeholder="التخصص"></asp:TextBox>
                                    <label for="<%= txtMajor.ClientID %>">التخصص</label>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvMajor" runat="server" ControlToValidate="txtMajor"
                                    ErrorMessage="يرجى إدخال التخصص" ForeColor="Red" Display="Dynamic" FontSize="Small"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="col-md-12 mb-4">
                            <label class="form-label">الحالة الأكاديمية</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- اختر الحالة --" Value=""></asp:ListItem>
                                <asp:ListItem Text="طالبة" Value="طالبة"></asp:ListItem>
                                <asp:ListItem Text="خريجة" Value="خريجة"></asp:ListItem>
                                <asp:ListItem Text="موظفة" Value="موظفة"></asp:ListItem>
                                <asp:ListItem Text="من خارج الجامعة" Value="من خارج الجامعة"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                InitialValue="" ErrorMessage="يرجى اختيار الحالة الأكاديمية" ForeColor="Red" Display="Dynamic" FontSize="Small"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-12 text-center">
                            <asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-lg btn-primary px-5 w-100" OnClick="btnSubmit_Click">
            تقديم الطلب
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            
            
                <div id="divSuccess" visible="false" runat="server" class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-8 text-center">
            <div class="card shadow-lg border-0 rounded-lg">
                <img src="https://pnu.edu.sa/Style Library/Images/AnnualEvent.jpeg" class="card-img-top" alt="Banner">
                
                <div runat="server" id="DivSucWrap" visible="false" class="card-body p-5">
                    <h3 class="card-title fw-bold text-primary mb-3">اتسجيل الدخول لحفل التميز 1446 هـ</h3>
                    <p class="text-muted mb-4">
                        تسجيل رقم <span class="badge bg-dark fs-6">#<asp:Label runat="server" ID="lblID"></asp:Label></span>
                    </p>

                    <hr class="my-4">

                    <div class="mb-4">
                        <div class="p-3 d-inline-block bg-light border rounded">
                            <asp:Image ID="ImageGeneratedBarcode" runat="server" CssClass="img-fluid" style="max-width: 250px;" />
                        </div>
                    </div>

                    <div class="mb-4">
                        <h4 class="fw-bold mb-1">
                            <asp:Label ID="lblName" runat="server"></asp:Label>
                        </h4>
                        <p class="text-secondary small">
                            <asp:Label ID="Lblnote" runat="server"></asp:Label>
                        </p>
                    </div>

                    <div class="alert alert-success d-flex align-items-center justify-content-center" role="alert">
                        <i class="bi bi-check-circle-fill me-2"></i>
                        <div>
                            <strong>تم التسجيل بنجاح!</strong> يرجى الاحتفاظ بالكود أعلاه وإظهاره أثناء الدخول.
                        </div>
                    </div>

                    <div class="mt-4 no-print">
                        <button type="button" class="btn btn-outline-primary px-4" onclick="window.print();">
                            <i class="bi bi-printer"></i> طباعة التذكرة
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
            </div>

        </div>
    </div>

</section>
