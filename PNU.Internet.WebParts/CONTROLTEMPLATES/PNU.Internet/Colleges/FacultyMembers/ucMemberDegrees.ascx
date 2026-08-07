<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMemberDegrees.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers.ucMemberDegrees" %>




<div class="row g-4">
    <div class="col-12 col-md-4">
        <article class="card nav-card h-100">
            <div class="card-body">
                <div class="icon-container"><i class="hgi hgi-stroke hgi-mortarboard-01 fs-3" aria-hidden="true"></i></div>
                <div>
                    <h3 class="card-title">
                        <asp:Literal ID="ltrBachelorTitle" runat="server"></asp:Literal>
                    </h3>
                    <p class="card-text mb-0"><asp:Literal ID="ltrBachelor" runat="server"></asp:Literal></p>
                </div>
            </div>
        </article>
    </div>
    <div class="col-12 col-md-4">
        <article class="card nav-card h-100">
            <div class="card-body">
                <div class="icon-container"><i class="hgi hgi-stroke hgi-mortarboard-01 fs-3" aria-hidden="true"></i></div>
                <div>
                    <h3 class="card-title">
                        <asp:Literal ID="ltrMasterTitle" runat="server"></asp:Literal>
                    </h3>
                    <p class="card-text mb-0"><asp:Literal ID="ltrMaster" runat="server"></asp:Literal></p>
                </div>
            </div>
        </article>
    </div>
    <div class="col-12 col-md-4">
        <article class="card nav-card h-100">
            <div class="card-body">
                <div class="icon-container"><i class="hgi hgi-stroke hgi-mortarboard-01 fs-3" aria-hidden="true"></i></div>
                <div>
                    <h3 class="card-title">
                        <asp:Literal ID="ltrDoctorateTitle" runat="server"></asp:Literal>
                    </h3>
                    <p class="card-text mb-0"><asp:Literal ID="ltrDoctorate" runat="server"></asp:Literal></p>
                </div>
            </div>
        </article>
    </div>
</div>
