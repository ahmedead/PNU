<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCommonContactUs.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common.ucCommonContactUs" %>

<script src="https://www.google.com/recaptcha/api.js" async defer></script>

<style type="text/css">
    .common-contact-us-wrapper .invalid-feedback-msg {
        color: #d9534f;
        font-size: 13px;
        margin-top: 5px;
        display: block;
    }
    .common-contact-us-wrapper .invalid-feedback-msg[style*="display: none"],
    .common-contact-us-wrapper .invalid-feedback-msg[style*="display:none"] {
        display: none !important;
    }

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

<div id="divWrapper" runat="server" class="common-contact-us-wrapper">
    <asp:Panel ID="pnlData" runat="server">
        <div class="container">
            <div>
                <div class="g-4 row">
                    <div class="col-12 col-lg-8">
                        <div class="my-4">
                            <div id="contactForm">
                                <div class="row">
                                    <!-- الاسم الكامل -->
                                    <div class="col-md-6 mb-4">
                                        <asp:Label ID="lblFullName" runat="server" AssociatedControlID="txtFullName" CssClass="form-label text-sm-semibold"></asp:Label>
                                        <div class="form-control-container">
                                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                                ValidationGroup="CommonContactUs" CssClass="invalid-feedback-msg" Display="Dynamic" />
                                        </div>
                                    </div>

                                    <!-- البريد الإلكتروني -->
                                    <div class="col-md-6 mb-4">
                                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="form-label text-sm-semibold"></asp:Label>
                                        <div class="form-control-container">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                ValidationGroup="CommonContactUs" CssClass="invalid-feedback-msg" Display="Dynamic" />
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                                ValidationGroup="CommonContactUs" ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$"
                                                CssClass="invalid-feedback-msg" Display="Dynamic" />
                                        </div>
                                    </div>

                                    <!-- تصنيف الاستفسار -->
                                    <div class="col-md-6 mb-4">
                                        <asp:Label ID="lblInquiryCategory" runat="server" AssociatedControlID="ddlInquiryCategory" CssClass="form-label text-sm-semibold"></asp:Label>
                                        <div class="form-control-container">
                                            <asp:DropDownList ID="ddlInquiryCategory" runat="server" CssClass="form-select">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvInquiryCategory" runat="server" ControlToValidate="ddlInquiryCategory"
                                                InitialValue="0" ValidationGroup="CommonContactUs" CssClass="invalid-feedback-msg" Display="Dynamic" />
                                        </div>
                                    </div>

                                    <!-- رقم الطلب (اختياري) -->
                                    <div class="col-md-6 mb-4">
                                        <asp:Label ID="lblApplicationNumber" runat="server" AssociatedControlID="txtApplicationNumber" CssClass="form-label text-sm-semibold"></asp:Label>
                                        <div class="form-control-container">
                                            <asp:TextBox ID="txtApplicationNumber" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>

                                    <!-- نص الرسالة -->
                                    <div class="col-12 mb-4">
                                        <asp:Label ID="lblMessage" runat="server" AssociatedControlID="txtMessage" CssClass="form-label text-sm-semibold"></asp:Label>
                                        <div class="form-control-container">
                                            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage"
                                                ValidationGroup="CommonContactUs" CssClass="invalid-feedback-msg" Display="Dynamic" />
                                        </div>
                                    </div>

                                    <!-- reCaptcha -->
                                    <div class="col-12 mb-4">
                                        <div class="form-group w-100">
                                            <div class="g-recaptcha" data-sitekey="6Leg0vAsAAAAAI9NROAGxiBaSjQD4UMc7eBzL7GL"></div>
                                        </div>
                                    </div>
                                </div>

                                <!-- زر الإرسال -->
                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" OnClick="btnSubmit_Click" ValidationGroup="CommonContactUs" />
                            </div>

                            <asp:Label ID="lblException" runat="server" CssClass="invalid-feedback-msg mt-3" Visible="false" />
                        </div>
                    </div>

                    <!-- Side Card -->
                    <div class="col-12 col-lg-4">
                        <div class="card e-service-side-nav">
                            <div class="card-body p-5">
                                <div class="d-flex flex-column gap-3">
                                    <div>
                                        <h4 class="fw-bold mb-1 h5"><asp:Literal ID="ltrSideTitle" runat="server" /></h4>
                                    </div>
                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-link-04"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="ltrPlatformTitle" runat="server" /></h4>
                                            <a class="external-link" href="https://tawasulnourah.pnu.edu.sa/" target="_blank" rel="noopener noreferrer">tawasulnourah.pnu.edu.sa</a>
                                        </div>
                                    </div>
                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class="hgi hgi-stroke hgi-call"></i>
                                        </span>
                                        <div>
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="ltrTawasulPhoneTitle" runat="server" /></h4>
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
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="ltrTawasulEmailTitle" runat="server" /></h4>
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
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="ltrUnivPhoneTitle" runat="server" /></h4>
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
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="ltrUnivEmailTitle" runat="server" /></h4>
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
                                            <h4 class="fw-bold mb-1 h6"><asp:Literal ID="ltrServicesTitle" runat="server" /></h4>
                                            <p class="mb-0"><asp:Literal ID="ltrServicesDesc" runat="server" /></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="success-panel text-center py-5">
        <div class="success-icon display-1 text-success mb-3">✓</div>
        <h3 class="text-primary mb-2"><asp:Literal ID="ltrSuccessTitle" runat="server" /></h3>
        <p class="lead mb-0"><asp:Literal ID="ltrSuccessDesc" runat="server" /></p>
        <asp:Label ID="lblSuccessMessage" runat="server" CssClass="d-block mt-2 text-muted" />
    </asp:Panel>
</div>
