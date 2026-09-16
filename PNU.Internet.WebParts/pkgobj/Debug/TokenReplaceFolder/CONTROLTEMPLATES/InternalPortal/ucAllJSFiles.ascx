<%@ Assembly Name="PNU.Internet.WebParts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bbc777e63fab09a3" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllJSFiles.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.InternalPortal.ucAllJSFiles" %>


	<script type="text/javascript" src="/Style%20Library/NewPortalIntranet/js/jquery-3.4.1.min.js"></script>
<%--<script type="text/javascript" src="/Style Library/New/js/jquery-3.6.0.min.js"></script>--%>


      
    <script type="text/javascript" src="/Style Library/New/js/helpers.js"></script>
    <script type="text/javascript" src="/Style Library/New/js/scripts.js"></script>
	

<script src="/Style%20Library/Portal/js/pagination.min.js"></script>
	  <%--<script src="/Style%20Library/PortalNewStyle/js/rating.js"></script>--%>
      <script src="/Style%20Library/PortalNewStyle/js/utils.js"></script>
      <script src="/Style%20Library/PortalNewStyle/js/custom.js"></script>



<script src="/Style%20Library/PortalNewStyle/js/fancybox.umd.js"></script>
            <script>
                $(function () {
                    $('.gallery_group').click(function () {
                        var catId = $(this).attr('data-id');
                        Fancybox.bind('[data-fancybox="photo' + catId + '"]', {
                            // Your options go here
                        });
                        $("#album_" + catId + " img").first().click();

                    });
                    $('.groupheader').remove();
                });
            </script>
