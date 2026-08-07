<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Controls/Common/Captcha.ascx" TagPrefix="uc1" TagName="Captcha" %>



<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAIContactUS.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAIContactUS" %>



<div class="container">
    <div>
        <div class="g-4 row">
            <div class="col-12 col-lg-8">
                <div class="my-4">

                    <div id="contactForm" class="row">

                        <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green" Font-Size="Large" Visible="false" />
                        <asp:Label ID="lblException" runat="server" ForeColor="Red" Font-Size="Large" Visible="false" />

                        <!-- First Name -->
                        <div class="col-md-6 mb-4">
                            <asp:Label runat="server" AssociatedControlID="txtFirstName"
                                    Text="<%$ Resources: PNUres, lblFirstName %>" 
                                    CssClass="form-label" />
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="requiredMsg" 
                                ValidationGroup="AddRequest" ControlToValidate="txtFirstName" 
                                runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                        </div>

                        <!-- Last Name -->
                        <div class="col-md-6 mb-4">
                            <asp:Label runat="server" AssociatedControlID="txtLastName"
                                    Text="<%$ Resources: PNUres, lblLastName %>" 
                                    CssClass="form-label" />
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="requiredMsg" 
                                ValidationGroup="AddRequest" ControlToValidate="txtLastName" 
                                runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                        </div>

                        <!-- Email -->
                        <div class="col-md-6 mb-4">
                            <asp:Label runat="server" AssociatedControlID="txtEmail"
                                    Text="<%$ Resources: PNUres, lblEmailAddress %>" 
                                    CssClass="form-label" />
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="requiredMsg" 
                                ValidationGroup="AddRequest" ControlToValidate="txtEmail" 
                                runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                        </div>

                        <!-- Mobile Number -->
                        <div class="col-md-6 mb-4">
                            <asp:Label runat="server" AssociatedControlID="txtMobileNo"
                                    Text="<%$ Resources: PNUres, lblPhoneNumber %>" 
                                    CssClass="form-label" />
                            <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="requiredMsg" 
                                ValidationGroup="AddRequest" ControlToValidate="txtMobileNo" 
                                runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                        </div>

                        <!-- Message -->
                        <div class="col-md-12 mb-4">
                            <asp:Label runat="server" AssociatedControlID="txtMessege"
                                    Text="<%$ Resources: PNUres, lblMessege %>" 
                                    CssClass="form-label" />
                            <asp:TextBox ID="txtMessege" runat="server" CssClass="form-control" TextMode="MultiLine" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="requiredMsg" 
                                ValidationGroup="AddRequest" ControlToValidate="txtMessege" 
                                runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic" />
                        </div>

                        <!-- Captcha -->
                        <div class="col-md-6 mb-4">
                            <uc1:Captcha runat="server" ValidationGroup="AddRequest" id="CaptchaControl" />
                        </div>

                        <!-- Submit Button -->
                        <div class="col-md-12 mb-4">
                            <asp:Button ID="btnSend" runat="server" CssClass="btn btn-primary" 
                                        ValidationGroup="AddRequest" 
                                        Text="<%$ Resources: PNUres, btnSend %>" 
                                        OnClick="btnSend_Click" />

                                        
                        </div>

                    </div>

                </div>
            </div>
            <div class="col-12 col-lg-4">

                <div class="card e-service-side-nav">
                    <div class="card-body p-5">
                        <div class="d-flex flex-column gap-3">
                            <div>
                                <h4 class="fw-bold mb-1 h5">تواصل نورة</h4>
                                <p class="mb-0 text-body-secondary">
                                    قناة موحدة للتواصل مع مسؤولي ومنسوبي الجامعة ورفع الطلبات ومتابعتها.
                                </p>
                            </div>
                            <div class="d-flex gap-2 align-items-start">
                                <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                    <i class="hgi hgi-stroke hgi-link-square-02"></i>
                                </span>
                                <div>
                                    <h4 class="fw-bold mb-1 h6">رابط الخدمة</h4>
                                    <a class="external-link" href="https://tawasulnourah.pnu.edu.sa/" target="_blank" rel="noopener noreferrer">
                                        <span>tawasulnourah.pnu.edu.sa</span>
                                    </a>
                                </div>
                            </div>
                            <div class="d-flex gap-2 align-items-start">
                                <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                    <i class="hgi hgi-stroke hgi-call"></i>
                                </span>
                                <div>
                                    <h4 class="fw-bold mb-1 h6">هاتف تواصل نورة</h4>
                                    <a class="external-link" href="tel:0118220000">
                                        <span>0118220000</span>
                                    </a>
                                </div>
                            </div>
                            <div class="d-flex gap-2 align-items-start">
                                <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                    <i class="hgi hgi-stroke hgi-mail-01"></i>
                                </span>
                                <div>
                                    <h4 class="fw-bold mb-1 h6">بريد تواصل نورة</h4>
                                    <a class="external-link" href="mailto:pnu-tawasul@pnu.edu.sa">
                                        <span>pnu-tawasul@pnu.edu.sa</span>
                                    </a>
                                </div>
                            </div>
                            <div class="d-flex gap-2 align-items-start">
                                <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                    <i class="hgi hgi-stroke hgi-call"></i>
                                </span>
                                <div>
                                    <h4 class="fw-bold mb-1 h6">هاتف الجامعة</h4>
                                    <a class="external-link" href="tel:+966118220000">
                                        <span>+966 11 822 0000</span>
                                    </a>
                                </div>
                            </div>
                            <div class="d-flex gap-2 align-items-start">
                                <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                    <i class="hgi hgi-stroke hgi-mail-01"></i>
                                </span>
                                <div>
                                    <h4 class="fw-bold mb-1 h6">بريد الجامعة</h4>
                                    <a class="external-link" href="mailto:info@pnu.edu.sa">
                                        <span>info@pnu.edu.sa</span>
                                    </a>
                                </div>
                            </div>
                            <div class="d-flex gap-2 align-items-start">
                                <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                    <i class="hgi hgi-stroke hgi-customer-service-01"></i>
                                </span>
                                <div>
                                    <h4 class="fw-bold mb-1 h6">الخدمات المتاحة</h4>
                                    <p class="mb-0">حجز المواعيد، ورفع الاستفسارات والشكاوى والمقترحات، ومتابعة
                                        الطلبات مع الجهات المعنية في الجامعة.</p>
                                </div>
                            </div>


                            <div class="d-flex gap-2 align-items-start">
                                <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                    <i class="hgi hgi-stroke hgi-location-01"></i>
                                </span>
                                <div>
                                    <h4 class="fw-bold mb-1 h6">العنوان الوطني</h4>
                                    <p class="mb-0">رقم المبنى 7808، مطار الملك خالد الدولي، رقم الوحدة 1،
                                        الرياض 13412-3230، المملكة العربية السعودية.</p>
                                    <p class="mb-0 mt-2">
                                        <span class="fw-bold">العنوان الوطني المختصر:</span>
                                        <span dir="ltr">RUKA7808</span>
                                    </p>
                                </div>
                            </div>
                            <!-- <div><a
                                class="btn btn-secondary solid"
                                href="/"
                                target="_blank"
                                >تحميل دليل المستخدم</a
                            ></div> -->
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>







<style>

	h4, h5, h6, .ms-h4, .ms-h5, .ms-h6 {
		font-family:"IBM Plex Sans Arabic", sans-serif;
	}


	.e-service-side-nav {
		top: -10rem;
	}
	
	@media (max-width: 576px){
		.e-service-side-nav {
			top: unset;
		}
	}
	

</style>