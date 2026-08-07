<%@ Assembly Name="PNU.Workflow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d7c9a0875b8f4acc" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestsUserActionsUserControl.ascx.cs" Inherits="PNU.Workflow.WebParts.RequestsUserActions.RequestsUserActionsUserControl" %>



<div class="messagealert" id="alert_container">
   
</div>
<section class="mb-4">
    <div class="container">
        <div class="row justify-content-center">

            <div class="col-lg-7 mb-4 mb-lg-5 news-form">
                <div class="card mb-4 p-5 shadow border-0 rounded-4 ">
                    <div class="row" id="dv_userAction" runat="server">
                        <div class="col-md-12">
                            <div class="mb-4 form-group">
                                <label>التعليق  </label>
                                <asp:TextBox ID="txtComments" Rows="3" TextMode="MultiLine" Height="300" placeholder="Content Here" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="mb-4">
                                <div class="form-group">
                                    <asp:Button ID="btnApprove" runat="server" Text="موافقة" CssClass="btn btn-primary mx-2" OnClick="btnApprove_Click" />
                                    <asp:Button ID="btnReject" runat="server" Text="رفض" CssClass="btn btn-danger mx-2" OnClick="btnReject_Click" />
                                    <asp:Button ID="brnReturn" runat="server" Text="إعادة" CssClass="btn alert-info d-none" OnClick="brnReturn_Click" Enabled="false" />

                                </div>
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>

</section>

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

<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.6/dist/umd/popper.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/js/bootstrap.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/swiper@8/swiper-bundle.min.js"></script>
  <script src="/_catalogs/masterpage/PortalUI/js/icons-svg.js"></script>

  <script src="/_catalogs/masterpage/PortalUI/js/main.js"></script>

  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>
  
<script>
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
