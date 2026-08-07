<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimerJobsManual.aspx.cs" Inherits="PNU.Internet.WebParts.Layouts.PNU.Internet.TimerJobsManual" DynamicMasterPageFile="~masterurl/Portal_Internal.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">


    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnRunCources" runat="server" Text="المواد الدراسية للأعضاء" OnClick="btnRunCources_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>



    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnGadeerAPICall" runat="server" Text="Gadeer API CALL" OnClick="btnGadeerAPICall_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>


    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnGadeerAPICall_EN" runat="server" Text="Gadeer API CALL EN" OnClick="btnGadeerAPICall_EN_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>


    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnRunCourcesToList" runat="server" Text="RunCourcesToList" OnClick="btnRunCourcesToList_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>

    <div class="col-md-12 align-items-stretch text-center">

    <span class="input-group-btn">
        <asp:Button ID="btnRunCourcesToList_EN" runat="server" Text="RunCourcesToList_EN" OnClick="btnRunCourcesToList_EN_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
    </span>


</div>


    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnStatistics" runat="server" Text="Statistics" OnClick="btnStatistics_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>

    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnFacultyMembers" runat="server" Text="أعضاء هيئة التدريس" OnClick="btnFacultyMembers_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>

    

    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnAddColleges" runat="server" Text="الكليات - الأقسام - البرامج" OnClick="btnAddCollegesNew_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>

    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnAddNewStudyPlan" runat="server" Text="المتطلبات للجامعة - الكلية - البرنامج" OnClick="btnAddNewStudyPlan_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>

    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnNewStudyPlan_M" runat="server" Text="الخطط الدراسية" OnClick="btnAddNewStudyPlanOld_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>


    <div class="col-md-12 align-items-stretch text-center">

        <span class="input-group-btn">
            <asp:Button ID="btnAcademicCredits" runat="server" Text="الاعتمادات الأكاديمية" OnClick="btnAcademicCredits_Click" class="btn btn-lg btn-primary rounded-pill px-5" />
        </span>


    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Timer Jobs Manual Run
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    My Application Page
</asp:Content>
