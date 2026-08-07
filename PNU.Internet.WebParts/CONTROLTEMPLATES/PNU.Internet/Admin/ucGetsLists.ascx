<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGetsLists.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Admin.ucGetsLists" %>


<style>
    .form-group.col-md-4 {
    padding-top: 20px;
}
</style>

<style>
    /* Make the DataTable container scrollable */
    .dataTables_wrapper {
        width: 100%;
        overflow-x: auto;
    }

    /* Ensure the table fills the container */
    table.dataTable {
        width: 100% !important;
    }
</style>

<div class="col-md-12 col-sm-12">
    <div class="comp-wp">
        <div class="row">
            <asp:PlaceHolder ID="phSubsiteDropdowns" runat="server">


                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel6" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel7" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel8" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel9" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>
                <div class="form-group col-md-4">
                    <asp:DropDownList ID="ddlSubsiteLevel10" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubsite_SelectedIndexChanged" Visible="false" CssClass="form-control" ></asp:DropDownList>
                </div>

            </asp:PlaceHolder>
			
			
        </div>

        
        <div class="col-md-6 col-sm-12">
            <asp:DropDownList ID="ddlLists" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLists_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        
</div>

    <div class="col-md-6 col-sm-12">
    <div class="col-md-6 col-sm-12">
        <div class="col-md-6 col-sm-12">
            <h4>بيانات الجدول</h4>

            <div class="col-md-6 col-sm-12">




                <div style="overflow: auto; width: 100%;">
                    <asp:GridView ID="GridView1" runat="server" CssClass="display compact" AutoGenerateColumns="true">
                    </asp:GridView>
                </div>


            </div>
        </div>
    </div>

</div>

    <asp:Literal ID="litScript" runat="server" />



  <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />

<!-- DataTables CSS -->
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css"/>
<!-- DataTables Buttons CSS -->
<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/buttons/2.4.1/css/buttons.dataTables.min.css"/>

<!-- jQuery -->
<script src="https://code.jquery.com/jquery-3.7.0.min.js"></script>
<!-- DataTables JS -->
<script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
<!-- DataTables Buttons JS -->
<script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.flash.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/pdfmake.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/vfs_fonts.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.html5.min.js"></script>
<script src="https://cdn.datatables.net/buttons/2.4.1/js/buttons.print.min.js"></script>


<script src="https://code.jquery.com/jquery-3.7.1.js"></script>
<script src="https://cdn.datatables.net/2.1.8/js/dataTables.js"></script>



<script type="text/javascript">
    $(function () {
        $('#<%= GridView1.ClientID %>').DataTable({
            bLengthChange: true,
            lengthMenu: [[5, 10, -1], [5, 10, "All"]],
            bFilter: true,
            bSort: true,
            bPaginate: true,
            dom: 'Bfrtip',
            scrollX: true,
            scrollY: '400px',
            scrollCollapse: true,
            paging: true,
            autoWidth: false,
            buttons: [
                {
                    extend: 'copy',
                    text: 'Copy',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'csv',
                    text: 'CSV',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'excel',
                    text: 'Excel',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'pdf',
                    text: 'PDF',
                    exportOptions: {
                        columns: ':visible'
                    }
                },
                {
                    extend: 'print',
                    text: 'Print',
                    exportOptions: {
                        columns: ':visible'
                    }
                }
            ]

        });


    });
</script>
