<%@ Assembly Name="PNU.Workflow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d7c9a0875b8f4acc" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestFormUserControl.ascx.cs" Inherits="PNU.Workflow.WebParts.RequestForm.RequestFormUserControl" %>


<link rel="stylesheet" href="/Style Library/Portal/css/custom.css" />

<style>
    .messagealert {
               width: 100%;
               position: fixed;
               top: 0px;
               z-index: 100000;
               padding: 0;
               font-size: 15px;
    }
</style>

<div class="messagealert" id="alert_container">
</div>
<div id="dvRequestsList" class="messagealert" runat="server" visible="false">
    <a href="requestList.aspx">Requests List</a>
</div>




<div id="dvMain" runat="server">

    <section class=" pt-5 mt-5 mb-4">
        <div class="container">
            <div class="row justify-content-center">

                <div class="col-lg-7 mb-4 mb-lg-5 news-form">
                    <div class="card mb-4 p-5 shadow border-0 rounded-4 ">
                        <div class="row">
                            <div class="col-md-12 text-start">
                                <h4 class="card-title mb-4 fw-bold">نموذج اضافة خبر
                  </h4>
                            </div>

                            <div class="col-md-12">
                                <div class="mb-4">

                                    <label>Media Category</label>

                                    <asp:DropDownList ID="ddlCategory" runat="server" class="form-select"></asp:DropDownList>

                                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" Enabled="true" ErrorMessage="<%$Resources:PNU-WF,res_required %>" ValidationGroup="group1" ControlToValidate="ddlCategory" InitialValue="0" CssClass="required" Style="color: red" Display="dynamic" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-4">

                                    <label>Media Type</label>

                                    <asp:CheckBoxList ID="chkTypes" runat="server" CssClass="form-form-control" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                    <asp:CustomValidator ID="rfvchkTypes" runat="server" Enabled="true" ClientValidationFunction="ValidateMediaTypes"
                                        Text="<%$Resources:PNU-WF,res_required %>" ErrorMessage="<%$Resources:PNU-WF,res_required %>"
                                        ValidationGroup="group1" Display="Dynamic" Style="color: red" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="mb-4">

                                    <label>Date</label>

                                    <SharePoint:DateTimeControl ID="publishingDate" runat="server" CssClassTextBox="form-control" IsRequiredField="true" />

                                </div>

                            </div>
                            <div class="col-md-12">
                                <div class="mb-4">

                                    <label>Faculty Name</label>

                                    <asp:Label ID="lblFacultyName" runat="server"></asp:Label>

                                </div>

                            </div>

                            <div class="col-md-12">
                                <div class=" mb-4 ">
                                    <label>Upload Image</label>

                                    <div class="input-group mb-4 ">

                                        <asp:FileUpload ID="fileUPload" runat="server" class="form-control h-100 m-0" />
                                        <asp:RequiredFieldValidator ID="rfvNewsImage" runat="server" Enabled="true"
                                            Text="<%$Resources:PNU-WF,res_required %>" ErrorMessage="<%$ Resources:PNU-WF,res_required %>"
                                            ControlToValidate="fileUPload" ValidationGroup="group1" Display="Dynamic" />
                                        <asp:RegularExpressionValidator ID="regNewsImage" runat="server" SetFocusOnError="true"
                                            ControlToValidate="fileUPload" Display="Dynamic" ValidationExpression="(.*).(.jpg|.JPG|.JPEG|.jpeg|.PNG|.png|.mp4)$"
                                            ErrorMessage="<%$Resources:PNU-WF,res_required %>"
                                            ValidationGroup="group1" Style="color: red" />
                                    </div>
                                </div>

                            </div>

                            <div class="col-md-12">
                                <div class="form-floating mb-4">

                                    <asp:TextBox ID="txtTitle" runat="server" class="form-control" placeholder=" Media Title Arabic"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="rvfTitle" runat="server" Enabled="true" ErrorMessage="<%$Resources:PNU-WF,res_required %>" ValidationGroup="group1" Style="color: red" ControlToValidate="txtTitle" CssClass="required" Display="dynamic" />
                                    <label for="floatingInputValue">Media Title Arabic</label>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="form-floating mb-4">

                                    <asp:TextBox ID="txtTitleEn" runat="server" class="form-control" placeholder=" Media Title English"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Enabled="true" ErrorMessage="<%$Resources:PNU-WF,res_required %>" ValidationGroup="group1" Style="color: red" ControlToValidate="txtTitleEn" CssClass="required" Display="dynamic" />
                                    <label for="floatingInputValue">Media Title English</label>
                                </div>
                            </div>

                            <div class="col-md-12">

                                <div class="form-floating mb-4">

                                    <asp:TextBox ID="txtSummary" placeholder=" Media Summary Arabic" TextMode="MultiLine" runat="server" Rows="3" class="form-control"></asp:TextBox>


                                    <asp:RequiredFieldValidator ID="rvfSummary" runat="server" Enabled="true" ErrorMessage="<%$Resources:PNU-WF,res_required %>" ValidationGroup="group1" ControlToValidate="txtSummary" CssClass="required" Style="color: red" Display="dynamic" />

                                    <label for="floatingTextarea2">Media Summary Arabic</label>

                                </div>
                            </div>

                            <div class="col-md-12">

    <div class="form-floating mb-4">

        <asp:TextBox ID="TextBox1" placeholder=" Media Summary" TextMode="MultiLine" runat="server" Rows="3" class="form-control"></asp:TextBox>


        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Enabled="true" ErrorMessage="<%$Resources:PNU-WF,res_required %>" ValidationGroup="group1" ControlToValidate="txtSummary" CssClass="required" Style="color: red" Display="dynamic" />

        <label for="floatingTextarea2">Media Summary</label>

    </div>
</div>



                            <div class="col-md-12">

                                <div class="form-floating mb-4">

                                    <asp:TextBox ID="txtDetails" placeholder=" Media Content" TextMode="MultiLine"  Height="300" runat="server" Rows="10" class="form-control"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="rvfDetails" runat="server" Enabled="true" ErrorMessage="<%$Resources:PNU-WF,res_required %>" ValidationGroup="group1" ControlToValidate="txtDetails" CssClass="required" Style="color: red" Display="dynamic" />

                                    <label for="floatingTextarea2">Media Content</label>
                                </div>
                            </div>

                            <div class="col-md-12 align-items-stretch text-center">

							<span class="input-group-btn">
				  <asp:Button ID="btnSubmit" runat="server" ValidationGroup="group1" Text="<%$Resources:PNU-WF,res_sendRequest %>" OnClick="btnSubmit_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
                </span>
				
                                
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>


    </section>

</div>


<script>
    function ValidateMediaTypes(source, args) {
        var chkListModules = document.getElementById('<%= chkTypes.ClientID %>');
        var chkListinputs = chkListModules.getElementsByTagName("input");
        for (var i = 0; i < chkListinputs.length; i++) {
            if (chkListinputs[i].checked) {
                args.IsValid = true;
                return;
            }
        }
        args.IsValid = false;
    }

    function ShowMessage(message, messagetype) {
        var cssclass;
        switch (messagetype) {
            case 'Success':
                cssclass = 'alert-success'
                break;
            case 'Error':
                cssclass = 'alert-danger'
                break;
            case 'Warning':
                cssclass = 'alert-warning'
                break;
            default:
                cssclass = 'alert-info'
        }
        $('#alert_container').append('<div id="alert_div" style="margin: 0 0.5%; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
    }
</script>
