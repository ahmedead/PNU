<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucColleges.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.Colleges.ucColleges" %>





<style>

.modal-confirm {		
	color: #636363;
	width: 400px;
}
.modal-confirm .modal-content {
	padding: 20px;
	border-radius: 5px;
	border: none;
	text-align: center;
	font-size: 14px;
}
.modal-confirm .modal-header {
	border-bottom: none;   
	position: relative;
}
.modal-confirm h4 {
	text-align: center;
	font-size: 26px;
	margin: 30px 0 -10px;
}

.modal-confirm .modal-body {
	color: #999;
}
.modal-confirm .modal-footer {
	border: none;
	text-align: center;		
	border-radius: 5px;
	font-size: 13px;
	padding: 10px 15px 25px;
}
.modal-confirm .modal-footer a {
	color: #999;
}		
.modal-confirm .icon-box {
	width: 80px;
	height: 80px;
	margin: 0 auto;
	border-radius: 50%;
	z-index: 9;
	text-align: center;
	border: 3px solid #f15e5e;
}
.modal-confirm .icon-box i {
	color: #f15e5e;
	font-size: 46px;
	display: inline-block;
	margin-top: 13px;
}

.contact__form__footer {
    padding: 35px;
}

</style>

<input id="ListItemID" type="hidden" value=""  runat="server"/>
<input id="SelectedItemID" type="hidden" value=""  runat="server"/>

<asp:Panel ID="pnlFaculties" runat="server" CssClass="tab-content mcbr-content-inbox overflow-hidden p-20">


    <div class="col-md-12 col-sm-12">
        <div class="comp-wp">
            <div class="row">
                <div class="form-group col-md-6">
                    <asp:Label ID="lblCategory" AssociatedControlID="ddlCategory" runat="server" CssClass="form-label required" Text="المجموعة"></asp:Label>
                    <asp:DropDownList DataValueField="ID" ID="ddlCategory" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--اختر--" Value="-1">--اختر--</asp:ListItem>
                        <asp:ListItem Text="الكليات الانسانية" Value="الكليات الانسانية">الكليات الانسانية</asp:ListItem>
                        <asp:ListItem Text="الكليات العلمية" Value="الكليات العلمية">الكليات العلمية</asp:ListItem>
                        <asp:ListItem Text="الكليات الصحية" Value="الكليات الصحية">الكليات الصحية</asp:ListItem>
                        <asp:ListItem Text="المعاهد" Value="المعاهد">المعاهد</asp:ListItem>
                        <asp:ListItem Text="برامج الدبلوم" Value="برامج الدبلوم">برامج الدبلوم</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="ddlCategory" InitialValue="-1" runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group col-md-6">
                    <asp:Label ID="lblFaculties" AssociatedControlID="ddlFaculties" runat="server" CssClass="form-label required" Text="الكليات"></asp:Label>
                    <asp:DropDownList DataValueField="ID" ID="ddlFaculties" AutoPostBack="true" OnSelectedIndexChanged="ddlFaculties_SelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="requiredMsg" ValidationGroup="AddRequest" ControlToValidate="ddlFaculties" InitialValue="-1" runat="server" ErrorMessage="الحقل مطلوب" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="contact__form__footer">
                <asp:Button CssClass="btn btn-lg btn-primary rounded-pill px-5" ID="btnNew" runat="server" Text="btnNew" data-toggle="modal" data-target="#MyPopup" OnClientClick="return false;"></asp:Button>
                <asp:Button CssClass="btn btn-lg btn-primary rounded-pill px-5" ID="btnUpdateExiting" ValidationGroup="AddRequest" runat="server" Text="btnUpdateExiting" data-toggle="modal" data-target="#MyPopupUpdate" OnClientClick="return false;"></asp:Button>
                <asp:Button CssClass="btn btn-lg btn-primary rounded-pill px-5" ID="btnDeleteExisting" ValidationGroup="AddRequest" runat="server" Text="btnDeleteExisting" data-toggle="modal" data-target="#DeleteModel" OnClientClick="return false;"></asp:Button>
            </div>
        </div>
    </div>



<!-- Modal New Popup -->
<div id="MyPopup" class="modal fade" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">تسجيل كلية جديدة
                </h4>
                

            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="form-group col-md-12">
                        <asp:Label ID="lblNameAr" AssociatedControlID="txtNameAr" runat="server" CssClass="required" meta:resourcekey="lblNameAr"></asp:Label>
                        <asp:TextBox ID="txtNameAr" runat="server" MaxLength="150" ValidationGroup="AddRequest" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="AddRequest" ID="RFVNameAr" CssClass="requiredMsg" ControlToValidate="txtNameAr" Display="Dynamic" runat="server" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                    </div>

                </div>
				
				<div class="row">
                    
                    <div class="form-group col-md-12">
                        <asp:Label ID="lblNameEn" AssociatedControlID="txtNameEn" runat="server" CssClass="required" meta:resourcekey="lblNameEn"></asp:Label>
                        <asp:TextBox ID="txtNameEn" runat="server" MaxLength="150" ValidationGroup="AddRequest" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="AddRequest" ID="RFVNameEn" CssClass="requiredMsg" ControlToValidate="txtNameEn" Display="Dynamic" runat="server" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                    </div>

                </div>
				
				
                <div class="row">
                    <div class="form-group col-md-6">
                        <asp:Button CssClass="btn btn-primary" ID="btnNewItem" ValidationGroup="AddRequest" runat="server" meta:resourcekey="btnNewItem" OnClick="btnNewItem_Click"></asp:Button>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">
                    إغلاق</button>
            </div>
        </div>
    </div>
</div>

<!-- Modal Update HTML -->
<div id="MyPopupUpdate" class="modal fade" role="dialog">
    <input id="SelectedIDFaculties" type="hidden" value="" runat="server" />
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">تعديل الكلية
                </h4>
                

            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="form-group col-md-12">
                        <asp:Label ID="lblNameArUpdate" AssociatedControlID="txtNameArUpdate" runat="server" CssClass="required" meta:resourcekey="lblNameAr"></asp:Label>
                        <asp:TextBox ID="txtNameArUpdate" runat="server" MaxLength="150" ValidationGroup="AddRequest" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="UpdateRequest" ID="RequiredFieldValidator1" CssClass="requiredMsg" ControlToValidate="txtNameArUpdate" Display="Dynamic" runat="server" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                    </div>
					</div>
				
				<div class="row">
                    <div class="form-group col-md-12">
                        <asp:Label ID="lblNameEnUpdate" AssociatedControlID="txtNameEn" runat="server" CssClass="required" meta:resourcekey="lblNameEn"></asp:Label>
                        <asp:TextBox ID="txtNameEnUpdate" runat="server" MaxLength="150" ValidationGroup="AddRequest" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="UpdateRequest" ID="RequiredFieldValidator2" CssClass="requiredMsg" ControlToValidate="txtNameEnUpdate" Display="Dynamic" runat="server" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="row">
                    <div class="contact__form__footer">
                        <asp:Button CssClass="btn btn-primary" ID="btnUpdate" ValidationGroup="UpdateRequest" runat="server" Text="حفظ" OnClick="btnUpdate_Click"></asp:Button>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">
                    إغلاق</button>
            </div>
        </div>
    </div>
</div>

<!-- Modal Delete HTML -->
<div id="DeleteModel" class="modal fade">
	<div class="modal-dialog modal-confirm">
		<div class="modal-content">
			<div class="modal-header flex-column">
				
				<div class="icon-box">
					<i class="material-icons">&#xE5CD;</i>
				</div>	
				
				<h4 class="modal-title w-100">هل أنت متأكد ؟</h4>	
                
			</div>
			<div class="modal-body">
				<p>سوف يتم حذف القطاع</p>
			</div>
			<div class="modal-footer justify-content-center">
				
                <asp:Button CssClass="btn btn-primary" ID="btnDelete" ValidationGroup="AddRequest" runat="server" Text="تأكيد الحذف" OnClick="btnDelete_Click"></asp:Button>
				<button type="button" class="btn btn-secondary" data-dismiss="modal">إلغاء</button>
			</div>
		</div>
	</div>
	</div>

</asp:Panel>


