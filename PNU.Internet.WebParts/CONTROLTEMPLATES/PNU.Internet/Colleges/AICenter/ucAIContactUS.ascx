<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Controls/Common/Captcha.ascx" TagPrefix="uc1" TagName="Captcha" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAIContactUS.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAIContactUS" %>

<%
    bool isArabicLang = (SPContext.Current.Web.Language == 1025);
    string sendInquiryText = isArabicLang ? "إرسال استفسار" : "Send Inquiry";
%>

<div class="d-flex flex-column gap-4">
    <h2 class="mb-4 fw-semibold">
        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AIContactUS %>" />
    </h2>

    <div class="row g-4">
        <!-- Contact Form -->
        <div class="col-12 col-lg-7">
            <div class="card border rounded-3 p-4 shadow-sm">
                <h3 class="h5 fw-bold mb-3">
                    <%= sendInquiryText %>
                </h3>

                <!-- Alerts -->
                <asp:Label ID="lblSuccessMessage" runat="server" CssClass="alert alert-success d-block mb-4 p-3 rounded-2" Visible="false" />
                <asp:Label ID="lblException" runat="server" CssClass="alert alert-danger d-block mb-4 p-3 rounded-2" Visible="false" />

                <div id="contactForm" class="row g-3">
                    <!-- First Name -->
                    <div class="col-12 col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtFirstName"
                                Text="<%$ Resources: PNUres, lblFirstName %>" 
                                CssClass="form-label fw-medium" />
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="text-danger small mt-1 d-block" 
                            ValidationGroup="AddRequest" ControlToValidate="txtFirstName" 
                            runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                    </div>

                    <!-- Last Name -->
                    <div class="col-12 col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtLastName"
                                Text="<%$ Resources: PNUres, lblLastName %>" 
                                CssClass="form-label fw-medium" />
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="text-danger small mt-1 d-block" 
                            ValidationGroup="AddRequest" ControlToValidate="txtLastName" 
                            runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                    </div>

                    <!-- Email -->
                    <div class="col-12 col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtEmail"
                                Text="<%$ Resources: PNUres, lblEmailAddress %>" 
                                CssClass="form-label fw-medium" />
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="text-danger small mt-1 d-block" 
                            ValidationGroup="AddRequest" ControlToValidate="txtEmail" 
                            runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                    </div>

                    <!-- Mobile Number -->
                    <div class="col-12 col-md-6">
                        <asp:Label runat="server" AssociatedControlID="txtMobileNo"
                                Text="<%$ Resources: PNUres, lblPhoneNumber %>" 
                                CssClass="form-label fw-medium" />
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="text-danger small mt-1 d-block" 
                            ValidationGroup="AddRequest" ControlToValidate="txtMobileNo" 
                            runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                    </div>

                    <!-- Message -->
                    <div class="col-12">
                        <asp:Label runat="server" AssociatedControlID="txtMessege"
                                Text="<%$ Resources: PNUres, lblMessege %>" 
                                CssClass="form-label fw-medium" />
                        <asp:TextBox ID="txtMessege" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="text-danger small mt-1 d-block" 
                            ValidationGroup="AddRequest" ControlToValidate="txtMessege" 
                            runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                    </div>

                    <!-- Captcha -->
                    <div class="col-12">
                        <uc1:Captcha runat="server" ValidationGroup="AddRequest" id="CaptchaControl" />
                    </div>

                    <!-- Submit Button -->
                    <div class="col-12 mt-3">
                        <asp:Button ID="btnSend" runat="server" CssClass="btn btn-primary px-4 py-2" 
                                    ValidationGroup="AddRequest" 
                                    Text="<%$ Resources: PNUres, btnSend %>" 
                                    OnClick="btnSend_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Contact Directory & Tawasul Sidebar -->
        <div class="col-12 col-lg-5">
            <div class="card border-0 bg-primary-25 p-4 rounded-3 h-100">
                <div class="d-flex flex-column gap-4">
                    <div>
                        <div class="d-flex align-items-center gap-2 mb-2">
                            <div class="icon-container bg-white">
                                <i class="hgi hgi-stroke hgi-customer-service-01 fs-4 text-primary" aria-hidden="true"></i>
                            </div>
                            <h3 class="fw-bold mb-0 h5">تواصل نورة</h3>
                        </div>
                        <p class="mb-0 text-muted small lh-base">
                            قناة موحدة للتواصل مع مسؤولي ومنسوبي الجامعة ورفع الطلبات ومتابعتها.
                        </p>
                    </div>

                    <div class="d-flex flex-column gap-3">
                        <div class="d-flex gap-3 align-items-start">
                            <span class="d-inline-flex fs-4 text-primary mt-1">
                                <i class="hgi hgi-stroke hgi-link-square-02"></i>
                            </span>
                            <div>
                                <h4 class="fw-bold mb-0 h6">رابط الخدمة</h4>
                                <a class="external-link small" href="https://tawasulnourah.pnu.edu.sa/" target="_blank" rel="noopener noreferrer">
                                    <span>tawasulnourah.pnu.edu.sa</span>
                                </a>
                            </div>
                        </div>

                        <div class="d-flex gap-3 align-items-start">
                            <span class="d-inline-flex fs-4 text-primary mt-1">
                                <i class="hgi hgi-stroke hgi-call"></i>
                            </span>
                            <div>
                                <h4 class="fw-bold mb-0 h6">هاتف تواصل نورة</h4>
                                <a class="external-link small" href="tel:0118220000">
                                    <span>0118220000</span>
                                </a>
                            </div>
                        </div>

                        <div class="d-flex gap-3 align-items-start">
                            <span class="d-inline-flex fs-4 text-primary mt-1">
                                <i class="hgi hgi-stroke hgi-mail-01"></i>
                            </span>
                            <div>
                                <h4 class="fw-bold mb-0 h6">بريد تواصل نورة</h4>
                                <a class="external-link small" href="mailto:pnu-tawasul@pnu.edu.sa">
                                    <span>pnu-tawasul@pnu.edu.sa</span>
                                </a>
                            </div>
                        </div>

                        <div class="d-flex gap-3 align-items-start">
                            <span class="d-inline-flex fs-4 text-primary mt-1">
                                <i class="hgi hgi-stroke hgi-building-03"></i>
                            </span>
                            <div>
                                <h4 class="fw-bold mb-0 h6">مركز الذكاء الاصطناعي</h4>
                                <a class="external-link small" href="mailto:ccis-aic@pnu.edu.sa">
                                    <span>ccis-aic@pnu.edu.sa</span>
                                </a>
                            </div>
                        </div>

                        <div class="d-flex gap-3 align-items-start">
                            <span class="d-inline-flex fs-4 text-primary mt-1">
                                <i class="hgi hgi-stroke hgi-location-01"></i>
                            </span>
                            <div>
                                <h4 class="fw-bold mb-0 h6">العنوان الوطني</h4>
                                <p class="mb-0 small text-muted">جامعة الأميرة نورة بنت عبد الرحمن، الرياض، المملكة العربية السعودية.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>